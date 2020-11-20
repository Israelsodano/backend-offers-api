using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.Repository;
using Common.Result;
using Common.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Constants;
using Offers.Domain.Entities;
using Offers.Domain.Entities.Enums;
using Offers.Domain.Services;

namespace Offers.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork<Offer> _unitOffer;
        private readonly IUnitOfWork<Course> _unitCourse;
        private readonly IValidator<Offer> _validator;
        public OfferService(IUnitOfWork<Offer> unitOffer,
                             IUnitOfWork<Course> unitCourse,
                             IValidator<Offer> validator)
        {
            _unitOffer = unitOffer ?? throw new ArgumentNullException(nameof(unitOffer));
            _unitCourse = unitCourse ?? throw new ArgumentNullException(nameof(unitCourse));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        private Expression<Func<Offer, dynamic>> GetOrderBy(string propertyName)
        {
            propertyName = propertyName.ToLower();

            if (nameof(Offer.EnrollmentSemester).ToLower().Equals(propertyName))
                return x => x.EnrollmentSemester;

            if (nameof(Offer.FullPrice).ToLower().Equals(propertyName))
                return x => x.FullPrice;

            if (nameof(Offer.PriceWithDiscount).ToLower().Equals(propertyName))
                return x => x.PriceWithDiscount;

            if (nameof(Offer.StartDate).ToLower().Equals(propertyName))
                return x => x.StartDate;

            return x => x.EnrollmentSemester;
        }

        private async Task<bool> CourseExistsById(Guid id) => (await _unitCourse.Repository.GetAll()).Any(x => x.Id == id);

        public async Task<IEnumerable<Offer>> GetPagedOffers(int range,
                                                                int page,
                                                                Offer search,
                                                                string orderBy,
                                                                bool desc = false)
        {
            Expression<Func<Offer, bool>> searchPredicate = x => ((string.IsNullOrEmpty(search.EnrollmentSemester) || x.EnrollmentSemester == search.EnrollmentSemester) &&
                                                                  (search.DiscountPercentage == 0 || x.DiscountPercentage == search.DiscountPercentage) &&
                                                                  (search.FullPrice == 0 || x.FullPrice == search.FullPrice) &&
                                                                  (search.PriceWithDiscount == 0 || x.PriceWithDiscount == search.PriceWithDiscount) &&
                                                                  (search.StartDate == DateTime.MinValue || x.StartDate == search.StartDate &&
                                                                  (search.CourseId == Guid.Empty || x.CourseId == search.CourseId)));

            var queryable = (await _unitOffer.Repository.GetAll(searchPredicate)).Include(@$"{nameof(Offer.Course)}.{nameof(Offer.Course.Campus)}.{nameof(Offer.Course.Campus.University)}");

            int count = (await _unitOffer.Repository.GetAll(searchPredicate)).Count();

            var result = PagedEntities.GetPagedEntities(page, range, count, desc, queryable, GetOrderBy(orderBy ?? string.Empty));

            if (result is null)
                return new Offer[] { };

            return await result.ToArrayAsync();
        }

        public async Task<Result<Offer>> CreateOffer(Offer Offer)
        {
            var validation = _validator.Validate(Offer);

            if (!(await CourseExistsById(Offer.CourseId)))
                return Result.Fail<Offer>(string.Format(ErrorMessages.COURSE_NOT_EXISTS, Offer.CourseId));

            if (!validation.IsValid)
                return Result.Fail<Offer>(validation.Errors.GetErrorMessage());

            if ((await _unitOffer.Repository.GetAll()).Any(x => x.CourseId == Offer.CourseId && x.DiscountPercentage == Offer.DiscountPercentage))
                return Result.Fail<Offer>(string.Format(ErrorMessages.OFFER_ALREADY_EXISTS, Offer.DiscountPercentage, Offer.CourseId));

            Offer.PriceWithDiscount = Offer.FullPrice * (Offer.DiscountPercentage / 100);

            await _unitOffer.Repository.AddAsync(Offer);
            await _unitOffer.CommitAsync();

            return Result.Ok(Offer);
        }

        public async Task RemoveOffer(Guid id)
        {
            await _unitOffer.Repository.DeleteAsync(x => x.Id == id);
            await _unitOffer.CommitAsync();
        }

        public async Task<Result<Offer>> UpdateOffer(Guid Id, Offer Offer, Semester semester, int year, bool? enabled)
        {
            var validation = _validator.Validate(Offer);

            if (!Offer.CourseId.Equals(Guid.Empty) && !(await CourseExistsById(Offer.CourseId)))
                return Result.Fail<Offer>(string.Format(ErrorMessages.COURSE_NOT_EXISTS, Offer.CourseId));

            if (!validation.IsValid)
                return Result.Fail<Offer>(validation.Errors.GetErrorMessage());

            if ((await _unitOffer.Repository.GetAll()).Any(x => x.CourseId == Offer.CourseId && x.DiscountPercentage == Offer.DiscountPercentage))
                return Result.Fail<Offer>(string.Format(ErrorMessages.OFFER_ALREADY_EXISTS, Offer.DiscountPercentage, Offer.CourseId));

            var OfferBase = await _unitOffer.Repository.GetFirst(x => x.Id == Id);

            if (OfferBase is null)
                return Result.Fail<Offer>(string.Format(ErrorMessages.OFFER_NOT_EXISTS, Id));

            OfferBase.DiscountPercentage = Offer.DiscountPercentage;
            OfferBase.FullPrice = Offer.FullPrice;
            OfferBase.PriceWithDiscount = Offer.FullPrice * (Offer.DiscountPercentage / 100);
            OfferBase.SetEnrollmentSemester(semester, year);
            OfferBase.StartDate = Offer.StartDate;
            OfferBase.Enabled = enabled ?? OfferBase.Enabled;
            OfferBase.CourseId = Offer.CourseId.Equals(Guid.Empty) ?
                                            OfferBase.CourseId :
                                            Offer.CourseId;

            await _unitOffer.Repository.UpdateAsync(OfferBase);
            await _unitOffer.CommitAsync();

            return Result.Ok(OfferBase);
        }
    }
}
