using System;
using System.Collections.Generic;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
	public class MessageBus : IMessageBus
	{
		private readonly Dictionary<Type, IMessagePipe> _messages = new Dictionary<Type, IMessagePipe>();

		public IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage
		{
			var message = GetOrAddMessage<MessagePipe<TMessage>>();
			return message.Register(listener);
		}

		public void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage
		{
			var message = GetMessage<MessagePipe<TMessage>>();
			if (message != null)
			{
				message.UnRegister(listener);
			}
		}

		public void Dispatch<T>() where T : new()
		{
			var message = GetMessage<MessagePipe<T>>();
			if (message != null)
			{
				message.Dispatch(new T());
			}
		}

		public void Dispatch<T>(T type)
		{
			var message = GetMessage<MessagePipe<T>>();
			if (message != null)
			{
				message.Dispatch(type);
			}
		}

		public void Clear()
		{
			_messages.Clear();
		}

		private T GetMessage<T>() where T : IMessagePipe
		{
			if (_messages.TryGetValue(typeof(T), out var message))
			{
				return (T)message;
			}

			return default;
		}

		private T GetOrAddMessage<T>() where T : IMessagePipe, new()
		{
			var type = typeof(T);
			if (_messages.TryGetValue(type, out var e))
			{
				return (T)e;
			}

			var t = new T();
			_messages.Add(type, t);
			return t;
		}
	}
}
