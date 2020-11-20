using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Repository;
using Offers.Domain.Entities.Enums;

namespace Offers.Domain.Entities
{
    public class Offer : EntityBase
    {
        public Guid CourseId { get; set; }
        public float FullPrice { get; set; }
        public float PriceWithDiscount { get; set; }
        public float DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public string EnrollmentSemester { get; private set; }
        public Offer SetEnrollmentSemester(Semester semester, int year)
        {
            if(year != 0)
                EnrollmentSemester = $"{year}.{(int)semester}";
           
            return this;
        }

        public bool Enabled { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
    }
}
