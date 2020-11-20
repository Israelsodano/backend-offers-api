using System;
using Common.Repository.Application.Events.Interfaces;

namespace Common.Repository.Application.Events
{
    internal sealed class DeleteCelebration : ICelebrityEvent
    {
        public string Name => nameof(DeleteCelebration);
        public EntityBase Celebrate(EntityBase entityBase)
        {
            entityBase.SetUpdatedAt(DateTime.Now);
            entityBase.SetAsDeleted();

            return entityBase;
        }
    }
}
