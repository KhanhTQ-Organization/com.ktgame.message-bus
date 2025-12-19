using System;
using com.ktgame.unregister;

namespace com.ktgame.message_bus
{
	public interface IMessagePipe
	{
		IUnRegister Register(Action listener);
	}
}
