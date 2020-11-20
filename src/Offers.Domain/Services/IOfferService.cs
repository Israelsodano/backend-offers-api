using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Offers.Domain.Entities;
using Offers.Domain.Entities.Enums;

namespace Offers.Domain.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<Offer>> GetPagedOffers(int range, 
                                                            int page,
                                                            Offer search, 
                                                            string orderBy,
                                                            bool desc = false);
        Task<Result<Offer>> CreateOffer(Offer Offer);
        Task<Result<Offer>> UpdateOffer(Guid Id, Offer Offer, Semester semester, int year, bool? enabled);
        Task RemoveOffer(Guid Id);
    }
}
