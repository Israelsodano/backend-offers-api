using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Offers.Domain.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Level
    {
        [Description("Bacharelado")]
        Bachelor,

        [Description("Tecnólogo")]
        Technologist,

        [Description("Licenciatura")]
        Degree
    }
}
