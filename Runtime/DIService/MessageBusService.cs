using System;
using com.ktgame.core;
using com.ktgame.unregister;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ktgame.message_bus
{
    [Service(typeof(IMessageBusService))]
    public class MessageBusService : MonoBehaviour, IMessageBusService
    {
        public int Priority => -1;
        public bool Initialized { get; set; }

        public IMessageBus MessageBus { get; private set; }

        public UniTask OnInitialize(IArchitecture architecture)
        {
            MessageBus = new MessageBus();
            Initialized = true;
            return UniTask.CompletedTask;
        }

        public IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
            return MessageBus.Register(listener);
        }

        public void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
            MessageBus.UnRegister(listener);
        }

        public void Dispatch<TMessage>() where TMessage : IMessage, new()
        {
            MessageBus.Dispatch<TMessage>();
        }

        public void Dispatch<TMessage>(TMessage message) where TMessage : IMessage
        {
            MessageBus.Dispatch(message);
        }

        public void Clear()
        {
            MessageBus.Clear();
        }

        public void OnSceneLoad(string sceneName)
        {
            Clear();
        }

        public void OnSceneUnload(string sceneName) { }
    }
}
