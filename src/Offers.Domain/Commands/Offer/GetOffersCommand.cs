using System;
using MediatR;
using Offers.Domain.Entities.Enums;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Offer
{
    public class GetOffersCommand : IRequest<BaseResponse>
    {
        public Guid CourseId { get; set; }
        public float FullPrice { get; set; }
        public float DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public Semester Semester { get; set; }
        public int EnrollmentSemesterYear { get; set; }

        public int Page { get; set; }
        public int Range { get; set; }
        public string OrderBy { get; set; }
        public bool Desc { get; set; } = false;
    }
}
