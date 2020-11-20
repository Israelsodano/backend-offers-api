using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Repository
{
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; private set; }
        public int EntityVersion { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        internal string EntityName => this.GetType().Name;
        internal DateTime SetCreatedAt(DateTime date) => CreatedAt = date;
        internal DateTime? SetUpdatedAt(DateTime date) => UpdatedAt = date;
        internal Guid SetId(Guid id) => Id = id;
        internal int IncrementVersion() => EntityVersion += 1;
        internal void SetAsDeleted() => EntityVersion = -1;
    }
}
