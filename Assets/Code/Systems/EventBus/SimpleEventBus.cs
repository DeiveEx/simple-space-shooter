using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EventBus.EditorTests")]
namespace Systems.EventBus
{
    public class SimpleEventBus : IEventBus
    {
        internal readonly Dictionary<Type, Dictionary<int, Action<IEvent>>> _handlers = new();

        public void Publish(IEvent @event)
        {
            var type = @event.GetType();

            if (!_handlers.TryGetValue(type, out var handlers))
                return;

            foreach (var handler in handlers.Values)
            {
                handler.Invoke(@event);
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);

            if (!_handlers.ContainsKey(type))
                _handlers.Add(type, new());

            _handlers[type].Add(handler.GetHashCode(), HandlerWrapper);

            //Wrap the handler into a method to allow us to register generic handlers
            void HandlerWrapper(IEvent e)
            {
                handler((T) e);
            }
        }

        public void UnregisterHandler<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);

            if (!_handlers.ContainsKey(type))
                return;

            _handlers[type].Remove(handler.GetHashCode());

            if (_handlers[type].Count == 0)
                _handlers.Remove(type);
        }
    }

}