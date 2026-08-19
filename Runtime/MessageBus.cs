using System;
using System.Collections.Generic;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
	public class MessageBus : IMessageBus
	{
		private readonly Dictionary<Type, IMessagePipe> _messages = new Dictionary<Type, IMessagePipe>();
		private readonly Dictionary<string, Dictionary<Type, IMessagePipe>> _channelMessages = new Dictionary<string, Dictionary<Type, IMessagePipe>>();

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

		public void Dispatch<T>() where T : IMessage, new()
		{
			var message = GetMessage<MessagePipe<T>>();
			if (message != null)
			{
				message.Dispatch(new T());
			}
		}

		public void Dispatch<T>(T type) where T : IMessage
		{
			var message = GetMessage<MessagePipe<T>>();
			if (message != null)
			{
				message.Dispatch(type);
			}
		}

		public IUnRegister Register<TMessage>(string channel, Action<TMessage> listener) where TMessage : IMessage
		{
			var message = GetOrAddChannelMessage<MessagePipe<TMessage>>(channel);
			return message.Register(listener);
		}

		public void UnRegister<TMessage>(string channel, Action<TMessage> listener) where TMessage : IMessage
		{
			var message = GetChannelMessage<MessagePipe<TMessage>>(channel);
			if (message != null)
			{
				message.UnRegister(listener);
			}
		}

		public void Dispatch<T>(string channel) where T : IMessage, new()
		{
			var message = GetChannelMessage<MessagePipe<T>>(channel);
			if (message != null)
			{
				message.Dispatch(new T());
			}
		}

		public void Dispatch<T>(string channel, T type) where T : IMessage
		{
			var message = GetChannelMessage<MessagePipe<T>>(channel);
			if (message != null)
			{
				message.Dispatch(type);
			}
		}

		public void Clear()
		{
			_messages.Clear();
			_channelMessages.Clear();
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

		private T GetChannelMessage<T>(string channel) where T : IMessagePipe
		{
			if (_channelMessages.TryGetValue(channel, out var channelDict))
			{
				if (channelDict.TryGetValue(typeof(T), out var message))
				{
					return (T)message;
				}
			}

			return default;
		}

		private T GetOrAddChannelMessage<T>(string channel) where T : IMessagePipe, new()
		{
			if (!_channelMessages.TryGetValue(channel, out var channelDict))
			{
				channelDict = new Dictionary<Type, IMessagePipe>();
				_channelMessages.Add(channel, channelDict);
			}

			var type = typeof(T);
			if (channelDict.TryGetValue(type, out var e))
			{
				return (T)e;
			}

			var t = new T();
			channelDict.Add(type, t);
			return t;
		}
	}
}
