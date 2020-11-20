using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Offers.Domain.Entities;

namespace Offers.Domain.Services
{
    public interface IUniversityService
    {
        Task<IEnumerable<University>> GetPagedUniversities(int range, 
                                                            int page,
                                                            University search, 
                                                            string orderBy,
                                                            bool desc = false);
        Task<Result<University>> CreateUniversity(University university);
        Task<Result<University>> UpdateUniversity(Guid Id, University university);
        Task RemoveUniversity(Guid Id);
    }
}
