
namespace GoBet.Application.DTOs
{
    public class StartTripRequestDto
    {
        public string Destination { get; set; } = string.Empty;
        public List<Guid> TerminalIds { get; set; } = new();
    }
}
