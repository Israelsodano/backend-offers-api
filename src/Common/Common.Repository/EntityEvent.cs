using System;
using Newtonsoft.Json;

namespace Common.Repository
{
    internal sealed class EntityEvent
    {
        public Guid Id { get; private set; }

        public string EventName { get; private set; }

        public string EntityName { get; private set; }

        public byte[] EntityValue { get; private set; }

        public Guid EntityId { get; private set; }

        public int EntityVersion { get; private set; }

        public void SetEventName(string eventName) => EventName = eventName;

        public static implicit operator EntityEvent(EntityBase entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            return new EntityEvent
            {
                EntityId = entity.Id,
                EntityName = entity.EntityName,
                EntityValue = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })),
                EntityVersion = entity.EntityVersion,
                Id = Guid.NewGuid()
            };
        }
    }
}
