using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Repository;
using Offers.Domain.Entities.Enums;

namespace Offers.Domain.Entities
{
    public class Course : EntityBase
    {
        public Guid CampusId { get; set; }
        public string Name { get; set; }
        public Kind Kind { get; set; }
        public Level Level { get; set; }
        public Shift Shift { get; set; }

        [ForeignKey(nameof(CampusId))]
        public virtual Campus Campus { get; set; }
    }
}
