namespace FlightPlannerWeb.Models
{
    public class PageResult
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public Flight[] Items { get; set; }

        public PageResult(Flight[] flights)
        {
            Items = flights;
            TotalItems = flights.Length;
        }
    }
}
