using System;
using System.Collections.Generic;
using com.ktgame.unregister;
using UnityEngine;

namespace com.ktgame.message_bus
{
    public class MessagePipe : IMessagePipe
    {
        private readonly List<Action> _listeners = new List<Action>();

        public IUnRegister Register(Action listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
            return new UnRegister(() => UnRegister(listener));
        }

        public void UnRegister(Action listener)
        {
            _listeners.Remove(listener);
        }

        public void Dispatch()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                try
                {
                    _listeners[i]?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"[MessagePipe] Dispatch error: {e}");
                }
            }
        }
    }

    public class MessagePipe<T> : IMessagePipe
    {
        private readonly List<Action<T>> _listeners = new List<Action<T>>();

        public IUnRegister Register(Action<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
            return new UnRegister(() => UnRegister(listener));
        }

        public void UnRegister(Action<T> listener)
        {
            _listeners.Remove(listener);
        }

        public void Dispatch(T t)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                try
                {
                    _listeners[i]?.Invoke(t);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[MessagePipe<{typeof(T).Name}>] Dispatch error: {e}");
                }
            }
        }

        public IUnRegister Register(Action listener)
        {
            return Register(Action);
            void Action(T _) => listener();
        }
    }

    public class MessagePipe<T, K> : IMessagePipe
    {
        private readonly List<Action<T, K>> _listeners = new List<Action<T, K>>();

        public IUnRegister Register(Action<T, K> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
            return new UnRegister(() => UnRegister(listener));
        }

        public void UnRegister(Action<T, K> listener)
        {
            _listeners.Remove(listener);
        }

        public void Dispatch(T t, K k)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                try
                {
                    _listeners[i]?.Invoke(t, k);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[MessagePipe<{typeof(T).Name}, {typeof(K).Name}>] Dispatch error: {e}");
                }
            }
        }

        public IUnRegister Register(Action listener)
        {
            return Register(Action);
            void Action(T _, K __) => listener();
        }
    }

    public class MessagePipe<T, K, S> : IMessagePipe
    {
        private readonly List<Action<T, K, S>> _listeners = new List<Action<T, K, S>>();

        public IUnRegister Register(Action<T, K, S> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
            return new UnRegister(() => UnRegister(listener));
        }

        public void UnRegister(Action<T, K, S> listener)
        {
            _listeners.Remove(listener);
        }

        public void Dispatch(T t, K k, S s)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                try
                {
                    _listeners[i]?.Invoke(t, k, s);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[MessagePipe<{typeof(T).Name}, {typeof(K).Name}, {typeof(S).Name}>] Dispatch error: {e}");
                }
            }
        }

        public IUnRegister Register(Action listener)
        {
            return Register(Action);
            void Action(T _, K __, S ___) => listener();
        }
    }
}
