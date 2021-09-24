using System.Text.Json.Serialization;

namespace FlightPlannerWeb.Models
{
    public class Airport
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string airport { get; set; }
    }
}
