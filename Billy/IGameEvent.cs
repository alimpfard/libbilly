using System;
namespace Billy
{
	public interface IGameEvent<T> where T : EventArgs
    {
		event EventHandler<T> OnFire;
    }
}
