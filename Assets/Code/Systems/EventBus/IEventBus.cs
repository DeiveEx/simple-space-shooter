using System;

namespace Systems.EventBus
{
    public interface IEventBus
    {
        public void Publish(IEvent @event);
        public void RegisterHandler<T>(Action<T> handler) where T : IEvent;
        public void UnregisterHandler<T>(Action<T> handler) where T : IEvent;
    }

}