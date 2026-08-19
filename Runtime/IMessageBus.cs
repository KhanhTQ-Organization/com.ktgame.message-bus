using System;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
	public interface IMessageBus
	{
		IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

		void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

		void Dispatch<T>() where T : IMessage, new();

		void Dispatch<T>(T type) where T : IMessage;

		IUnRegister Register<TMessage>(string channel, Action<TMessage> listener) where TMessage : IMessage;

		void UnRegister<TMessage>(string channel, Action<TMessage> listener) where TMessage : IMessage;

		void Dispatch<T>(string channel) where T : IMessage, new();

		void Dispatch<T>(string channel, T type) where T : IMessage;

		void Clear();
	}
}
