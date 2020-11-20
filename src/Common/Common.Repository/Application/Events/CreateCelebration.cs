using System;
using Common.Repository.Application.Events.Interfaces;

namespace Common.Repository.Application.Events
{
    internal sealed class CreateCelebration : ICelebrityEvent
    {
        public string Name => nameof(CreateCelebration);

        public EntityBase Celebrate(EntityBase entityBase)
        {
            entityBase.SetId(Guid.NewGuid());
            entityBase.SetCreatedAt(DateTime.Now);

            return entityBase;
        }
    }
}
