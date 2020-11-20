using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Offers.Domain.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Kind
    {
        [Description("EaD")]
        DistanceLearning,

        [Description("Presencial")]
        Presential
    }
}
