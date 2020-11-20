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
    public class UniversityService : IUniversityService
    {
        private readonly IUnitOfWork<University> _unitUniversity;
        private readonly IValidator<University> _validator;

        public UniversityService(IUnitOfWork<University> unitUniversity,
                                 IValidator<University> validator)
        {
            _unitUniversity = unitUniversity ?? throw new ArgumentNullException(nameof(unitUniversity));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        private Expression<Func<University, dynamic>> GetOrderBy(string propertyName)
        {
            propertyName = propertyName.ToLower();
        
            if (nameof(Domain.Entities.University.Name).ToLower().Equals(propertyName))
                return x => x.Name;

            if (nameof(University.Score).ToLower().Equals(propertyName))
                return x => x.Score;

            return x => x.Name;
        }

        public async Task<IEnumerable<University>> GetPagedUniversities(int range, 
                                                                int page,
                                                                University search, 
                                                                string orderBy,
                                                                bool desc = false)
        {
            Expression<Func<University, bool>> searchPredicate = x=> ((string.IsNullOrEmpty(search.Name) || x.Name.ToLower().Contains(search.Name.ToLower())) &&
                                                                                     (search.Score.Equals(0) || x.Score.Equals(search.Score)));

            var queryable = await _unitUniversity.Repository.GetAll(searchPredicate);
            int count = (await _unitUniversity.Repository.GetAll(searchPredicate)).Count();

            var result = PagedEntities.GetPagedEntities(page, range, count, desc, queryable, GetOrderBy(orderBy ?? string.Empty));

            if (result is null)
                return new University[] { };

            return await result.ToArrayAsync();
        }

        public async Task<Result<University>> CreateUniversity(University university)
        {
            var validation = _validator.Validate(university);

            if (!validation.IsValid)
                return Result.Fail<University>(validation.Errors.GetErrorMessage());

            if((await _unitUniversity.Repository.GetAll()).Any(x=> x.Name.ToLower() == university.Name.ToLower()))
                return Result.Fail<University>(string.Format(ErrorMessages.UNIVERSITY_ALREADY_EXISTS, university.Name));

            await _unitUniversity.Repository.AddAsync(university);
            await _unitUniversity.CommitAsync();

            return Result.Ok(university);
        }

        public async Task RemoveUniversity(Guid id)
        {
            await _unitUniversity.Repository.DeleteAsync(x => x.Id == id);
            await _unitUniversity.CommitAsync();
        }

        public async Task<Result<University>> UpdateUniversity(Guid Id, University university)
        {
            var validation = _validator.Validate(university);

            if (!validation.IsValid)
                return Result.Fail<University>(validation.Errors.GetErrorMessage());

            var universityBase = await _unitUniversity.Repository.GetFirst(x => x.Id == Id);

            if(universityBase is null)
                return Result.Fail<University>(string.Format(ErrorMessages.UNIVERSITY_NOT_EXISTS, Id));

            universityBase.Name = university.Name;
            universityBase.Score = university.Score;
            universityBase.LogoUrl = university.LogoUrl;

            await _unitUniversity.Repository.UpdateAsync(universityBase);
            await _unitUniversity.CommitAsync();

            return Result.Ok(universityBase);
        }
    }
}
