using System;
using System.Collections.Generic;
using Common.Repository.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Application
{
    internal sealed class EventManeger : IEventManeger
    {
        private readonly IEventLabels _eventLables;
        private readonly IList<EntityEvent> _entityEvents;
        public EventManeger(IEventLabels eventLables)
        {
            _eventLables = eventLables ?? throw new ArgumentNullException(nameof(eventLables));
            _entityEvents = new List<EntityEvent>();
        }

        public void CelebrateEvent(EntityState state, EntityBase entityBase)
        {
            var @event = _eventLables[state];
            AddEvent(@event.Celebrate(entityBase), @event.Name);
        }

        public void CelebrateEvent(EntityState state, IEnumerable<EntityBase> entities)
        {
            var @event = _eventLables[state];
            foreach (var entity in entities)
                AddEvent(@event.Celebrate(entity), @event.Name);     
        }

        private void AddEvent(EntityEvent @event, string eventName)
        {
            @event.SetEventName(eventName);
            _entityEvents.Add(@event);
        }

        public IEnumerable<EntityEvent> GetCelebratedEvents() => _entityEvents;

        public void ClearEvents() => _entityEvents.Clear();

        public void Dispose()
        {
            GC.SuppressFinalize(_eventLables);
            GC.SuppressFinalize(_entityEvents);
        }
    }
}
