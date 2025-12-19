using System;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
 public class MessagePipe : IMessagePipe
    {
        private Action _listener = () => { };

        public IUnRegister Register(Action listener)
        {
            _listener += listener;
            return new UnRegister(() => { UnRegister(listener); });
        }

        public void UnRegister(Action listener)
        {
            _listener -= listener;
        }

        public void Dispatch()
        {
            _listener.Invoke();
        }
    }

    public class MessagePipe<T> : IMessagePipe
    {
        private Action<T> _listener = e => { };

        public IUnRegister Register(Action<T> listener)
        {
            _listener += listener;
            return new UnRegister(() => UnRegister(listener));
        }

        public void UnRegister(Action<T> listener)
        {
            _listener -= listener;
        }

        public void Dispatch(T t)
        {
            _listener?.Invoke(t);
        }

        public IUnRegister Register(Action listener)
        {
            return Register(Action);
            void Action(T _) => listener();
        }
    }

    public class MessagePipe<T, K> : IMessagePipe
    {
        private Action<T, K> _listener = (t, k) => { };

        public IUnRegister Register(Action<T, K> listener)
        {
            _listener += listener;
            return new UnRegister(() => { UnRegister(listener); });
        }

        public void UnRegister(Action<T, K> listener)
        {
            _listener -= listener;
        }

        public void Dispatch(T t, K k)
        {
            _listener?.Invoke(t, k);
        }

        public IUnRegister Register(Action listener)
        {
            return Register(Action);
            void Action(T _, K __) => listener();
        }
    }

    public class MessagePipe<T, K, S> : IMessagePipe
    {
        private Action<T, K, S> _listener = (t, k, s) => { };

        public IUnRegister Register(Action<T, K, S> listener)
        {
            _listener += listener;
            return new UnRegister(() => { UnRegister(listener); });
        }

        public void UnRegister(Action<T, K, S> listener)
        {
            _listener -= listener;
        }

        public void Dispatch(T t, K k, S s)
        {
            _listener?.Invoke(t, k, s);
        }

        public IUnRegister Register(Action listener)
        {
            return Register(Action);
            void Action(T _, K __, S ___) => listener();
        }
    }
}
