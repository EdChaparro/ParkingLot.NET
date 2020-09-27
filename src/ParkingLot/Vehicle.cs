using System;

namespace IntrepidProducts.ParkingLot
{
    public interface IVehicle
    {
        string Make { get;  }
        string License { get;  }
    }

    public class Vehicle : IVehicle
    {
        public Vehicle(string make, string license)
        {
            Make = make;
            License = license;
        }

        public string Make { get; }
        public string License { get; }

        #region Equality
        public override bool Equals(object? obj)
        {
            var vehicle = obj as IVehicle;

            return vehicle != null && Equals(vehicle);
        }

        protected bool Equals(IVehicle other)
        {
            return Make == other.Make && License == other.License;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Make, License);
        }
        #endregion

        public override string ToString()
        {
            return $"Make: {Make}, License: {License}";
        }
    }
}