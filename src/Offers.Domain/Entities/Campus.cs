using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Repository;

namespace Offers.Domain.Entities
{
    public class Campus : EntityBase
    {
        public Guid UniversityId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        [ForeignKey(nameof(UniversityId))]
        public virtual University University { get; set; }
    }
}
