using Common.Repository;

namespace Offers.Domain.Entities
{
    public class University : EntityBase
    {
        public string Name { get; set; }
        public float Score { get; set; }
        public string LogoUrl { get; set; }
    }
}
