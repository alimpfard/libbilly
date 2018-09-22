using System;
namespace Billy
{
	public class LoopRenderEvent : IGameEvent
	{
		public int Info;
		public EventManager manager;
		public object GetInfo()
		{
			return Info;
		}

		public EventManager GetManager()
		{
			return manager;
		}
	}
	public class preRenderEventT : LoopRenderEvent { }
	public class RenderEventT : LoopRenderEvent { }
	public class postRenderEventT : LoopRenderEvent { }

    public class EventLoop
    {
		readonly public EventManager eventManager = new EventManager();
		public EventManager.callbackMethod PreRender, OnRender, PostRender;

		public int phaseCount = 6;
		private RenderEventT renderPhaseEvt;
		private preRenderEventT preRenderEvt;
		private postRenderEventT postRenderEvt;

		public EventLoop(EventManager.callbackMethod preRender, EventManager.callbackMethod onRender, EventManager.callbackMethod postRender)
        {
			renderPhaseEvt = new RenderEventT { manager = eventManager };
			preRenderEvt = new preRenderEventT { manager = eventManager };
			postRenderEvt = new postRenderEventT { manager = eventManager };
			PreRender = preRender;
			OnRender = onRender;
			PostRender = postRender;
			// Register our own render functions
			if (preRender != null)
			    eventManager.Subscribe(typeof(preRenderEventT), PreRender);
			if (onRender != null)
				eventManager.Subscribe(typeof(RenderEventT), OnRender);
			if (postRender != null)
				eventManager.Subscribe(typeof(postRenderEventT), PostRender);
        }
		public void Process()
		{
			while (true)
			{
				eventManager.Notify(typeof(preRenderEventT), this, preRenderEvt);
				eventManager.Update();
				for (int phaseId = 0; phaseId < phaseCount; phaseId++)
				{
					renderPhaseEvt.Info = phaseId;
					eventManager.Notify(typeof(RenderEventT), this, renderPhaseEvt);
					eventManager.Update();
				}
				eventManager.Notify(typeof(postRenderEventT), this, postRenderEvt);
				eventManager.Update();
			}
		}
    }
}
