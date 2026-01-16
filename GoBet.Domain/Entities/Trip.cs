using System;
using System.Collections.Generic;
using System.Text;

namespace GoBet.Domain.Entities
{
    public class Trip(int totalSeats)
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public string Destination { get; set; } = string.Empty;
        public int AvailableSeats { get; private set; } = totalSeats;
        public bool IsFull => AvailableSeats <= 0;

        public void OccupySeat()
        {
            if (IsFull) throw new Exception("No seats available.");
            AvailableSeats--;
        }
    }
}
