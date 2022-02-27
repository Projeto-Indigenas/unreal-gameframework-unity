using System;

namespace UnrealEngine.Core
{
    internal interface IEventInvoker<TEvent>
    {
        void Invoke(TEvent evt);
    }

    internal class __RealEvent<TOwner, TEvent>
        where TOwner : IEventInvoker<TEvent>
        where TEvent : class
    {
        private TArray<WeakReference<TEvent>> _events = default;

        public void Broadcast(TOwner owner)
        {
            for (int index = _events.Num() - 1; index >= 0; index--)
            {
                WeakReference<TEvent> weak = _events[index];
                
                if (!weak.TryGetTarget(out TEvent evt))
                {
                    _events.RemoveAt(index);

                    continue;
                }

                owner.Invoke(evt);
            }
        }

        public static __RealEvent<TOwner, TEvent> operator +(__RealEvent<TOwner, TEvent> owner, TEvent evt)
        { 
            owner._events.Add(new WeakReference<TEvent>(evt));
            return owner;
        }
    }

    public struct FDeclareEvent : IEventInvoker<Action>
    {
        private __RealEvent<FDeclareEvent, Action> _eventPriv;

        private __RealEvent<FDeclareEvent, Action> _event
        {
            get => _eventPriv ??= new __RealEvent<FDeclareEvent, Action>();
            set { }
        }

        public void Broadcast()
        {
            _event.Broadcast(this);
        }

        void IEventInvoker<Action>.Invoke(Action evt)
        {
            evt.Invoke();
        }

        public static FDeclareEvent operator +(FDeclareEvent evt, Action action)
        {
            evt._event += action;
            return evt;
        }
    }

    public struct FDeclareEvent<TParam1> : IEventInvoker<Action<TParam1>>
    {
        private __RealEvent<FDeclareEvent<TParam1>, Action<TParam1>> _eventPriv;

        private TParam1 _param1;

        private __RealEvent<FDeclareEvent<TParam1>, Action<TParam1>> _event
        {
            get => _eventPriv ??= new __RealEvent<FDeclareEvent<TParam1>, Action<TParam1>>();
            set { }
        }

        public void Broadcast(TParam1 param1)
        {
            _param1 = param1;
            _event.Broadcast(this);
            _param1 = default;
        }

        void IEventInvoker<Action<TParam1>>.Invoke(Action<TParam1> evt)
        {
            evt.Invoke(_param1);
        }

        public static FDeclareEvent<TParam1> operator +(FDeclareEvent<TParam1> evt, Action<TParam1> action)
        {
            evt._event += action;
            return evt;
        }
    }

    public struct FDeclareEvent<TParam1, TParam2> : IEventInvoker<Action<TParam1, TParam2>>
    {
        private __RealEvent<FDeclareEvent<TParam1, TParam2>, Action<TParam1, TParam2>> _eventPriv;

        private TParam1 _param1;
        private TParam2 _param2;

        private __RealEvent<FDeclareEvent<TParam1, TParam2>, Action<TParam1, TParam2>> _event
        {
            get => _eventPriv ??= new __RealEvent<FDeclareEvent<TParam1, TParam2>, Action<TParam1, TParam2>>();
            set { }
        }

        public void Broadcast(TParam1 param1, TParam2 param2)
        {
            _param1 = param1;
            _param2 = param2;
            _event.Broadcast(this);
            _param1 = default;
            _param2 = default;
        }

        void IEventInvoker<Action<TParam1, TParam2>>.Invoke(Action<TParam1, TParam2> evt)
        {
            evt.Invoke(_param1, _param2);
        }

        public static FDeclareEvent<TParam1, TParam2> operator +(FDeclareEvent<TParam1, TParam2> evt, Action<TParam1, TParam2> action)
        {
            evt._event += action;
            return evt;
        }
    }

    public struct FDeclareEvent<TParam1, TParam2, TParam3> : IEventInvoker<Action<TParam1, TParam2, TParam3>>
    {
        private __RealEvent<FDeclareEvent<TParam1, TParam2, TParam3>, Action<TParam1, TParam2, TParam3>> _eventPriv;

        private TParam1 _param1;
        private TParam2 _param2;
        private TParam3 _param3;

        private __RealEvent<FDeclareEvent<TParam1, TParam2, TParam3>, Action<TParam1, TParam2, TParam3>> _event
        {
            get => _eventPriv ??= new __RealEvent<FDeclareEvent<TParam1, TParam2, TParam3>, Action<TParam1, TParam2, TParam3>>();
            set { }
        }

        public void Broadcast(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            _param1 = param1;
            _param2 = param2;
            _param3 = param3;
            _event.Broadcast(this);
            _param1 = default;
            _param2 = default;
            _param3 = default;
        }

        void IEventInvoker<Action<TParam1, TParam2, TParam3>>.Invoke(Action<TParam1, TParam2, TParam3> evt)
        {
            evt.Invoke(_param1, _param2, _param3);
        }

        public static FDeclareEvent<TParam1, TParam2, TParam3> operator +(
            FDeclareEvent<TParam1, TParam2, TParam3> evt, 
            Action<TParam1, TParam2, TParam3> action)
        {
            evt._event += action;
            return evt;
        }
    }
}
