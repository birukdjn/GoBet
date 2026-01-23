namespace GoBet.Application.DTOs
{
    public class NearbyBusesResponse
    {
        public string NearestTerminalName { get; set; } = string.Empty;
        public IEnumerable<TripDto> Buses { get; set; } = new List<TripDto>();
    }
}