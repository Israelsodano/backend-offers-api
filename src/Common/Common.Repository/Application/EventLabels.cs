using System.Collections.Generic;
using Common.Repository.Application.Events;
using Common.Repository.Application.Events.Interfaces;
using Common.Repository.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Application
{
    internal sealed class EventLabels : Dictionary<EntityState, ICelebrityEvent>, IEventLabels
    {
        public EventLabels()
        {
            Add(EntityState.Added, new CreateCelebration());
            Add(EntityState.Modified, new UpdateCelebration());
            Add(EntityState.Deleted, new DeleteCelebration());
        }
    }
}
