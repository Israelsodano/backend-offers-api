using System;
using Common.Result;
using MediatR;
using Offers.Domain.Entities.Enums;
using Offers.Domain.Responses.Base;

namespace Offers.Domain.Commands.Offer
{
    public class CreateOfferCommand : IRequest<Result<BaseResponse>>
    {
        public Guid CourseId { get; set; }
        public float FullPrice { get; set; }
        public float DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public Semester Semester { get; set; }
        public int EnrollmentSemesterYear { get; set; }
    }
}
