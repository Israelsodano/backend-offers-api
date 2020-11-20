using System;
using Common.Repository.Application.Events.Interfaces;

namespace Common.Repository.Application.Events
{
    internal sealed class UpdateCelebration : ICelebrityEvent
    {
        public string Name => nameof(UpdateCelebration);

        public EntityBase Celebrate(EntityBase entityBase)
        {
            entityBase.SetUpdatedAt(DateTime.Now);
            entityBase.IncrementVersion();

            return entityBase;
        }
    }
}
