using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Offers.Domain.Entities;

namespace Offers.Domain.Services
{
    public interface ICampusService
    {
        Task<IEnumerable<Campus>> GetPagedCampus(int range, 
                                                            int page,
                                                            Campus search, 
                                                            string orderBy,
                                                            bool desc = false);
        Task<Result<Campus>> CreateCampus(Campus Campus);
        Task<Result<Campus>> UpdateCampus(Guid Id, Campus Campus);
        Task RemoveCampus(Guid Id);
    }
}
