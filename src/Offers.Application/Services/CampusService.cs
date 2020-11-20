using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Repository;
using Common.Result;
using Common.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Constants;
using Offers.Domain.Entities;
using Offers.Domain.Services;

namespace Offers.Application.Services
{
    public class CampusService : ICampusService
    {
        private readonly IUnitOfWork<Campus> _unitCampus;
        private readonly IUnitOfWork<University> _unitUniversity;
        private readonly IValidator<Campus> _validator;

        public CampusService(IUnitOfWork<Campus> unitCampus,
                             IUnitOfWork<University> unitUniversity,
                             IValidator<Campus> validator)
        {
            _unitCampus = unitCampus ?? throw new ArgumentNullException(nameof(unitCampus));
            _unitUniversity = unitUniversity ?? throw new ArgumentNullException(nameof(unitUniversity));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        private Expression<Func<Campus, dynamic>> GetOrderBy(string propertyName)
        {
            propertyName = propertyName.ToLower();

            if (nameof(Campus.Name).ToLower().Equals(propertyName))
                return x => x.Name;

            if (nameof(Campus.City).ToLower().Equals(propertyName))
                return x => x.City;

            return x => x.Name;
        }

        private async Task<bool> UniversityExistsById(Guid id) => (await _unitUniversity.Repository.GetAll()).Any(x => x.Id == id);

        public async Task<IEnumerable<Campus>> GetPagedCampus(int range,
                                                                int page,
                                                                Campus search,
                                                                string orderBy,
                                                                bool desc = false)
        {
            Expression<Func<Campus, bool>> searchPredicate = x => ((string.IsNullOrEmpty(search.Name) || x.Name.ToLower().Contains(search.Name.ToLower())) &&
                                                                   (string.IsNullOrEmpty(search.City) || x.City.ToLower().Contains(search.City)) &&
                                                                   (search.UniversityId.Equals(Guid.Empty) || x.UniversityId == search.UniversityId));

            var queryable = await _unitCampus.Repository.GetAll(searchPredicate);
            int count = (await _unitCampus.Repository.GetAll(searchPredicate)).Count();

            var result = PagedEntities.GetPagedEntities(page, range, count, desc, queryable, GetOrderBy(orderBy ?? string.Empty));

            if (result is null)
                return new Campus[] { };

            return await result.ToArrayAsync();
        }

        public async Task<Result<Campus>> CreateCampus(Campus campus)
        {
            var validation = _validator.Validate(campus);

            if(!(await UniversityExistsById(campus.UniversityId)))
                return Result.Fail<Campus>(string.Format(ErrorMessages.UNIVERSITY_NOT_EXISTS, campus.UniversityId));

            if (!validation.IsValid)
                return Result.Fail<Campus>(validation.Errors.GetErrorMessage());

            if ((await _unitCampus.Repository.GetAll()).Any(x => x.Name.ToLower() == campus.Name.ToLower()))
                return Result.Fail<Campus>(string.Format(ErrorMessages.CAMPUS_ALREADY_EXISTS, campus.Name));

            await _unitCampus.Repository.AddAsync(campus);
            await _unitCampus.CommitAsync();

            return Result.Ok(campus);
        }

        public async Task RemoveCampus(Guid id)
        {
            await _unitCampus.Repository.DeleteAsync(x => x.Id == id);
            await _unitCampus.CommitAsync();
        }

        public async Task<Result<Campus>> UpdateCampus(Guid Id, Campus campus)
        {
            var validation = _validator.Validate(campus);
            
            if (!campus.UniversityId.Equals(Guid.Empty) && !(await UniversityExistsById(campus.UniversityId)))
                return Result.Fail<Campus>(string.Format(ErrorMessages.UNIVERSITY_NOT_EXISTS, campus.UniversityId));

            if (!validation.IsValid)
                return Result.Fail<Campus>(validation.Errors.GetErrorMessage());

            var campusBase = await _unitCampus.Repository.GetFirst(x => x.Id == Id);

            if (campusBase is null)
                return Result.Fail<Campus>(string.Format(ErrorMessages.CAMPUS_NOT_EXISTS, Id));

            campusBase.Name = campus.Name;
            campusBase.City = campus.City;
            campusBase.UniversityId = campus.UniversityId.Equals(Guid.Empty) ? 
                                            campusBase.UniversityId : 
                                            campus.UniversityId;

            await _unitCampus.Repository.UpdateAsync(campusBase);
            await _unitCampus.CommitAsync();

            return Result.Ok(campusBase);
        }
    }
}
