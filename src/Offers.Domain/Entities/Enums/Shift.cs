using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Offers.Domain.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Shift
    {
        [Description("Noite")]
        Nigth,

        [Description("Virtual")]
        Virtual,

        [Description("Manhã")]
        Morning
    }
}
