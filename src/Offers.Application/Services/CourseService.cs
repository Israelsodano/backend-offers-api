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
using Offers.Domain.Entities.Enums;
using Offers.Domain.Services;

namespace Offers.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork<Course> _unitCourse;
        private readonly IUnitOfWork<Campus> _unitCampus;
        private readonly IValidator<Course> _validator;

        public CourseService(IUnitOfWork<Course> unitCourse,
                             IUnitOfWork<Campus> unitCampus,
                             IValidator<Course> validator)
        {
            _unitCourse = unitCourse ?? throw new ArgumentNullException(nameof(unitCourse));
            _unitCampus = unitCampus ?? throw new ArgumentNullException(nameof(unitCampus));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        private async Task<bool> CampusExistsById(Guid id) => (await _unitCampus.Repository.GetAll()).Any(x => x.Id == id);

        public async Task<IEnumerable<Course>> GetPagedCourses(int range,
                                                                int page,
                                                                string name,
                                                                Kind? kind,
                                                                Level? level,
                                                                Shift? shift,
                                                                bool desc = false)
        {
            Expression<Func<Course, bool>> searchPredicate = x => ((string.IsNullOrEmpty(name) || x.Name.ToLower().Contains(name.ToLower())) && 
                                                                   (!kind.HasValue || x.Kind.Equals(kind.Value)) &&
                                                                   (!level.HasValue || x.Level.Equals(level.Value)) &&
                                                                   (!shift.HasValue || x.Shift.Equals(shift.Value)));

            var queryable = await _unitCourse.Repository.GetAll(searchPredicate);
            int count = (await _unitCourse.Repository.GetAll(searchPredicate)).Count();

            var result = PagedEntities.GetPagedEntities(page, range, count, desc, queryable, x=> x.Name);

            if (result is null)
                return new Course[] { };

            return await result.ToArrayAsync();
        }

        public async Task<Result<Course>> CreateCourse(Course course)
        {
            var validation = _validator.Validate(course);

            if (!(await CampusExistsById(course.CampusId)))
                return Result.Fail<Course>(string.Format(ErrorMessages.CAMPUS_NOT_EXISTS, course.CampusId));

            if (!validation.IsValid)
                return Result.Fail<Course>(validation.Errors.GetErrorMessage());

            if ((await _unitCourse.Repository.GetAll()).Any(x => x.Name.ToLower() == course.Name.ToLower()))
                return Result.Fail<Course>(string.Format(ErrorMessages.COURSE_ALREADY_EXISTS, course.Name));

            await _unitCourse.Repository.AddAsync(course);
            await _unitCourse.CommitAsync();

            return Result.Ok(course);
        }

        public async Task RemoveCourse(Guid id)
        {
            await _unitCourse.Repository.DeleteAsync(x => x.Id == id);
            await _unitCourse.CommitAsync();
        }

        public async Task<Result<Course>> UpdateCourse(Guid Id, Course course)
        {
            var validation = _validator.Validate(course);

            if (!course.CampusId.Equals(Guid.Empty) && !(await CampusExistsById(course.CampusId)))
                return Result.Fail<Course>(string.Format(ErrorMessages.CAMPUS_NOT_EXISTS, course.CampusId));

            if (!validation.IsValid)
                return Result.Fail<Course>(validation.Errors.GetErrorMessage());

            var courseBase = await _unitCourse.Repository.GetFirst(x => x.Id == Id);

            if (courseBase is null)
                return Result.Fail<Course>(string.Format(ErrorMessages.COURSE_NOT_EXISTS, Id));

            courseBase.Name = course.Name;
            courseBase.Kind = course.Kind;
            courseBase.Level = course.Level;
            courseBase.Shift = course.Shift;
            courseBase.CampusId = course.CampusId.Equals(Guid.Empty) ?
                                            courseBase.CampusId :
                                            course.CampusId;

            await _unitCourse.Repository.UpdateAsync(courseBase);
            await _unitCourse.CommitAsync();

            return Result.Ok(courseBase);
        }
    }
}
