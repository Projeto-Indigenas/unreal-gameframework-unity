using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;
using UnrealEngine.DeveloperSettings;
using UnrealEngine.Engine;

namespace UnrealEditor
{
    public class UProjectSettingsWindow : EditorWindow
    {
        private const string _baseDirectoryPath = "Assets/UnrealProject";
        private const string _baseSettingsPath = _baseDirectoryPath + "/Settings/";
        private const string _baseScenesPath = _baseDirectoryPath + "/Scenes/";
        private const string _settingsAssetPath = _baseSettingsPath + "ProjectSettings.asset";
        private const string _initializationScenePath = _baseScenesPath + "StartupLevel.unity";

        private static TArray<Type> _allDeveloperSettings = default;

        private ProjectSettingsScriptableObject _projectSettings = default;
        private TMap<string, FoldoutCategory> _foldouts = default;
        private Vector2 _categoriesPosition = default;
        private Vector2 _settingsPosition = default;

        private Editor[] _selectedSettingsEditors = default;

        static UProjectSettingsWindow()
        {
            AssemblyHelper.FindAll<UDeveloperSettings>(out _allDeveloperSettings);
        }

        [MenuItem("Window/Unreal/Show ProjectSettings")]
        public static void ShowSettings()
        {
            UProjectSettingsWindow window = GetWindow<UProjectSettingsWindow>();
            window.titleContent = new GUIContent("Unreal Project Settings");
            window.Show();
        }

        [MenuItem("Window/Unreal/Close ProjectSettings")]
        public static void CloseSettings()
        {
            UProjectSettingsWindow window = GetWindow<UProjectSettingsWindow>();
            if (!window) return;
            window.Close();
        }

        private void OnEnable()
        {
            void PrivateOnEnable()
            {
                EditorApplication.update -= PrivateOnEnable;

                if (Application.isPlaying) return;

                SelectSettings(null);

                InitializeConfigurations();

                if (!_projectSettings) return;

                EnsureScriptableObjectHasAllTypes();

                if (_selectedSettingsEditors != null) return;

                SelectSettings(_projectSettings.containers);
            }

            if (Application.isPlaying) return;

            EditorApplication.update += PrivateOnEnable;

            EditorApplication.QueuePlayerLoopUpdate();
        }

        private void InitializeConfigurations()
        {
            if (!Directory.Exists(_baseSettingsPath)) Directory.CreateDirectory(_baseSettingsPath);
            if (!Directory.Exists(_baseScenesPath)) Directory.CreateDirectory(_baseScenesPath);

            InitializeProject();

            CreateOrUpdateSceneIfNotExists();
        }

        private void CreateOrUpdateSceneIfNotExists()
        {
            if (File.Exists(_initializationScenePath))
            {
                Scene newScene = EditorSceneManager.OpenScene(_initializationScenePath, OpenSceneMode.Additive);

                CreateEngineObject(newScene);

                _ = EditorSceneManager.SaveScene(newScene, _initializationScenePath, false);

                _ = EditorSceneManager.CloseScene(newScene, true);
            }
            else
            {
                Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                
                CreateEngineObject(newScene);

                _ = EditorSceneManager.SaveScene(newScene, _initializationScenePath);

                _ = EditorSceneManager.CloseScene(newScene, true);
            }

            EditorBuildSettingsScene settingsScene = new EditorBuildSettingsScene(_initializationScenePath, true);
            TArray<EditorBuildSettingsScene> scenes = new TArray<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            int indexOfScene = scenes.FindIndex(each => each.guid == settingsScene.guid);
            if (indexOfScene != 0)
            {
                if (indexOfScene == -1)
                {
                    scenes.Insert(0, settingsScene);
                }
                else
                {
                    EditorBuildSettingsScene otherScene = scenes[0];
                    EditorBuildSettingsScene initScene = scenes[indexOfScene];
                    scenes[0] = initScene;
                    scenes[indexOfScene] = otherScene;
                }
                EditorBuildSettings.scenes = scenes;
            }

            void CreateEngineObject(Scene newScene)
            {
                bool foundInitScript = false;
                __EngineInitialization initScript;
                foreach (GameObject gameObject in newScene.GetRootGameObjects())
                {
                    if (foundInitScript = gameObject.TryGetComponent(out initScript)) return;
                }

                GameObject engineObject = new GameObject("UnrealEngine");
                __EngineInitialization script = engineObject.AddComponent<__EngineInitialization>();
                script.settingsScriptableObject = _projectSettings;

                SceneManager.MoveGameObjectToScene(engineObject, newScene);
            }
        }

        private void InitializeProject()
        {
            if (File.Exists(_settingsAssetPath))
            {
                _projectSettings = AssetDatabase.LoadAssetAtPath<ProjectSettingsScriptableObject>(_settingsAssetPath);
            }
            else
            {
                _projectSettings = CreateInstance<ProjectSettingsScriptableObject>();
                AssetDatabase.CreateAsset(_projectSettings, _settingsAssetPath);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Separator();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            {
                _categoriesPosition = EditorGUILayout.BeginScrollView(_categoriesPosition,
                    EditorStyles.helpBox, GUILayout.Width(200F), GUILayout.ExpandHeight(true));
                {
                    if (_foldouts != null)
                    {
                        foreach (KeyValuePair<string, FoldoutCategory> pair in _foldouts)
                        {
                            DoCategory(pair.Key, pair.Value);
                        }
                    }
                }
                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space(6F, false);

                _settingsPosition = EditorGUILayout.BeginScrollView(_settingsPosition,
                    EditorStyles.helpBox, GUILayout.ExpandHeight(true));
                {
                    if (_selectedSettingsEditors != null)
                    {
                        for (int index = 0; index < _selectedSettingsEditors.Length; index++)
                        {
                            Editor editor = _selectedSettingsEditors[index];
                            editor.DrawHeader();
                            editor.OnInspectorGUI();
                            EditorGUILayout.Space(50F);
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        private void DoCategory(string categoryName, FoldoutCategory category)
        {
            GUI.contentColor = Color.white;
            if (category.foldout = EditorGUILayout.BeginFoldoutHeaderGroup(category.foldout, categoryName))
            {
                EditorGUILayout.BeginVertical();
                {
                    foreach (KeyValuePair<string, FoldoutSection> pair in category.sections)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.Space(10F, false);
                            if (GUILayout.Button(pair.Key, EditorStyles.linkLabel))
                            {
                                SelectSettings(pair.Value.developerSettings.ToArray());
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void SelectSettings(DeveloperSettingsContainerScriptableObject[] settings)
        {
            if (_selectedSettingsEditors != null)
            {
                for (int index = 0; index < _selectedSettingsEditors.Length; index++)
                {
                    DestroyImmediate(_selectedSettingsEditors[index]);
                }
                _selectedSettingsEditors = null;
            }
            if (settings == null || settings.Length == 0) return;
            _selectedSettingsEditors = Array.ConvertAll(settings, each => Editor.CreateEditor(each));
        }

        private void EnsureScriptableObjectHasAllTypes()
        {
            TArray<Type> typesToAdd = new TArray<Type>(_allDeveloperSettings);

            _foldouts = new TMap<string, FoldoutCategory>();

            TArray<DeveloperSettingsContainerScriptableObject> allSettings = new TArray<DeveloperSettingsContainerScriptableObject>();
            if (_projectSettings.containers != null)
            {
                allSettings.AddRange(_projectSettings.containers);
            }

            for (int index = allSettings.Count - 1; index >= 0; index--)
            {
                DeveloperSettingsContainerScriptableObject container = allSettings[index];

                if (!container || container.settings == null)
                {
                    allSettings.RemoveAt(index);

                    continue;
                }

                UpdateNavigator(container);

                UClass cls = container.settings.GetClass();

                if (!typesToAdd.Contains(cls)) continue;

                typesToAdd.Remove(cls);
            }


            for (int index = 0; index < typesToAdd.Count; index++)
            {
                UClass cls = typesToAdd[index];

                DeveloperSettingsContainerScriptableObject container = CreateInstance<DeveloperSettingsContainerScriptableObject>();
                container.settings = UClass.NewObject<UDeveloperSettings>(cls);

                AssetDatabase.CreateAsset(container, Path.Combine(_baseSettingsPath, $"{container.typeName}.asset"));

                allSettings.Add(container);

                UpdateNavigator(container);
            }

            _projectSettings.containers = allSettings.ToArray();

            EditorUtility.SetDirty(_projectSettings);
        }

        private void UpdateNavigator(DeveloperSettingsContainerScriptableObject container)
        {
            FoldoutSection section;
            if (!_foldouts.TryGetValue(container.settings.categoryName, out FoldoutCategory category))
            {
                section = new FoldoutSection();
                section.developerSettings.Add(container);

                category = new FoldoutCategory();
                category.sections.Add(container.settings.sectionName, section);

                _foldouts.Add(container.settings.categoryName, category);

                return;
            }

            if (!category.sections.TryGetValue(container.settings.sectionName, out section))
            {
                section = new FoldoutSection();
                
                category.sections.Add(container.settings.sectionName, section);
            }

            section.developerSettings.Add(container);
        }

        private class FoldoutCategory
        {
            public readonly TMap<string, FoldoutSection> sections = default;

            public bool foldout = true;

            public FoldoutCategory()
            {
                sections = new TMap<string, FoldoutSection>();
            }
        }

        private class FoldoutSection
        {
            public readonly TArray<DeveloperSettingsContainerScriptableObject> developerSettings = default;

            public FoldoutSection()
            {
                developerSettings = new TArray<DeveloperSettingsContainerScriptableObject>();
            }
        }
    }
}
