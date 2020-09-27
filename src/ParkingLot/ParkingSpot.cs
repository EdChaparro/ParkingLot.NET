using System;

namespace IntrepidProducts.ParkingLot
{
    public class ParkingSpot
    {
        public ParkingSpot(string id, int floor = 0, string section = "default")
        {
            Id = id;
            Floor = floor;
            Section = section;
        }

        public string Id { get; }
        public string Section { get; }
        public int Floor { get; }

        #region Equality
        public override bool Equals(object? obj)
        {
            var parkingSpot = obj as ParkingSpot;

            return parkingSpot != null && Equals(parkingSpot);
        }

        protected bool Equals(ParkingSpot other)
        {
            return Id == other.Id && Section == other.Section && Floor == other.Floor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Section, Floor);
        }
        #endregion

        public override string ToString()
        {
            return $"Id: {Id}, Section: {Section}, Floor: {Floor}";
        }
    }
}