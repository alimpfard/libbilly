using System;
using System.Collections.Generic;

namespace Billy
{
	public interface IGameEvent
	{
		object GetInfo();
		EventManager GetManager();
	}
    public class EventManager
    {
		Dictionary<Type, List<callbackMethod>> subscribers = new Dictionary<Type, List<callbackMethod>>();
		public delegate void callbackMethod (object sender, IGameEvent @event);
		readonly PQ<float, Tuple<object, IGameEvent, callbackMethod>> toFire = new PQ<float, Tuple<object, IGameEvent, callbackMethod>>();

		public void Notify(Type id, object sender, IGameEvent @event)
		{
			Notify(id, sender, @event, 0);
		}
		public void Notify(Type id, object sender, IGameEvent @event, float priority)
		{ 
			if (subscribers.TryGetValue(id, out var events))
            {
				Type evtType = @events.GetType();
				events.ForEach((callbackMethod callback) =>
				               toFire.Enqueue(priority, Tuple.Create(sender, @event, callback)));
            }
            else return; // maybe throw?
		}
		public void Subscribe(Type id, callbackMethod callback)
		{
			List<callbackMethod> pairs;
			if (!subscribers.TryGetValue(id, out pairs))
			{
				pairs = new List<callbackMethod> { };
				subscribers.Add(id, pairs);
			}
			pairs.Add(callback);
		}
		public void Unsubscribe(Type id)
		{
			subscribers.Remove(id);
		}
		public void Unsubscribe(Type id, callbackMethod callback)
		{
            if (subscribers.TryGetValue(id, out var pairs))
            {
				pairs.Remove(callback);
            }
		}
		public void UnsubscribeAll()
		{
			subscribers.Clear();
		}
		public void Update()
		{
			while (toFire.Count > 0)
			{
				var v = toFire.Dequeue();
				v.Item3(v.Item1, v.Item2);
			}
		}
    }
}
