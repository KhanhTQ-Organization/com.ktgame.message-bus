using System;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
	public interface IMessageBus
	{
		IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

		void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

		void Dispatch<T>() where T : new();

		void Dispatch<T>(T type);

		void Clear();
	}
}
