using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Application.Interfaces
{
    internal interface IEventManeger : IDisposable
    {
        void CelebrateEvent(EntityState state, EntityBase entityBase);
        void CelebrateEvent(EntityState state, IEnumerable<EntityBase> entities);
        IEnumerable<EntityEvent> GetCelebratedEvents();
        void ClearEvents();
    }
}
