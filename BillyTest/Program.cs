using System;
using Billy;

namespace BillyTest
{
	public class Entity0
    {
        public struct PhaseTick
        {
            public void Invoke(object sender, IGameEvent _evt)
            {
                var evt = (RenderEventT)_evt;
                if (evt.Info > 3)
                {
                    Console.WriteLine("Entity ticked, current phase is " + evt.Info);
                }
            }
        }
        public PhaseTick phase;
    }
    public class Test
    {
        static EventLoop billy;
        static Entity0 entity = new Entity0();
        static void HandlecallbackMethod(object sender, IGameEvent @event)
        {
            Console.WriteLine("Now at preRender");
            var manager = @event.GetManager();
            manager.Subscribe(typeof(Entity0.PhaseTick), entity.phase.Invoke);
        }

        static void HandlecallbackMethod1(object sender, IGameEvent @event)
        {
			Console.WriteLine("Now at render");
			var manager = @event.GetManager();
			manager.Notify(typeof(Entity0.PhaseTick), null, @event);
        }

		static void HandlecallbackMethod2(object sender, IGameEvent @event)
		{
			Console.WriteLine("Now at postRender, sleeping");
			System.Threading.Thread.Sleep(100);
		}


        public static void Main(string[] args)
        {
			billy = new EventLoop(HandlecallbackMethod, HandlecallbackMethod1, HandlecallbackMethod2);
            billy.Process();
        }
    }
}
