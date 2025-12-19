using System;
using com.ktgame.core;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
    public interface IMessageBusService : IInitializable, ISceneLoad
    {
        IMessageBus MessageBus { get; }

        IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

        void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

        void Dispatch<TMessage>() where TMessage : IMessage, new();

        void Dispatch<TMessage>(TMessage message) where TMessage : IMessage;

        void Clear();
    }
}
