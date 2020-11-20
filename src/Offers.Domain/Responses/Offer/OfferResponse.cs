using EnumsNET;
using Newtonsoft.Json;


namespace Offers.Domain.Responses.Offer
{
    public class OfferResponse
    {
        [JsonProperty("full_price")]
        public float FullPrice { get; set; }

        [JsonProperty("price_with_discount")]
        public float PriceWithDiscount { get; set; }
        
        [JsonProperty("discount_percentage")]
        public float DiscountPercentage { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("enrollment_semester")]
        public string EnrollmentSemester { get; set; }

        public object Course { get; set; }
        public object University { get; set; }
        public object Campus { get; set; }

        public static implicit operator OfferResponse(Entities.Offer offer)
        {
            return offer is null ? null : new OfferResponse
            {
                FullPrice = offer.FullPrice,
                DiscountPercentage = offer.DiscountPercentage,
                EnrollmentSemester = offer.EnrollmentSemester,
                StartDate = offer.StartDate.ToString("dd/MM/yyyy"),
                PriceWithDiscount = offer.PriceWithDiscount,
                Course = new
                {
                    offer.Course.Name,
                    Kind = offer.Course.Kind.AsString(EnumFormat.Description),
                    Shift = offer.Course.Shift.AsString(EnumFormat.Description),
                    Level = offer.Course.Level.AsString(EnumFormat.Description),
                },
                Campus = new
                {
                    offer.Course.Campus.Name,
                    offer.Course.Campus.City
                },
                University = new
                {
                    offer.Course.Campus.University.Name,
                    offer.Course.Campus.University.LogoUrl,
                    offer.Course.Campus.University.Score
                }
            };
        }
    }
}
