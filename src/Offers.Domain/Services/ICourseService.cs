using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Result;
using Offers.Domain.Entities;
using Offers.Domain.Entities.Enums;

namespace Offers.Domain.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetPagedCourses(int range,
                                                    int page,
                                                    string name,
                                                    Kind? kind,
                                                    Level? level,
                                                    Shift? shift,
                                                    bool desc = false);
        Task<Result<Course>> CreateCourse(Course Course);
        Task<Result<Course>> UpdateCourse(Guid Id, Course Course);
        Task RemoveCourse(Guid Id);
    }
}
