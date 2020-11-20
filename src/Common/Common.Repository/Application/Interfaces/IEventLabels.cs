using System.Collections.Generic;
using Common.Repository.Application.Events.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Application.Interfaces
{
    internal interface IEventLabels : IDictionary<EntityState, ICelebrityEvent>
    {

    }
}
