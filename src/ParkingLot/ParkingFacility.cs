using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IntrepidProducts.ParkingLot
{
    public interface IParkingFacility : IEnumerable<ParkingSpot>
    {
        IList<ParkingSpot> All { get; }
    }

    public class ParkingFacility : IParkingFacility
    {
        private List<ParkingSpot> _spots = new List<ParkingSpot>();

        public IList<ParkingSpot> All => new List<ParkingSpot>(_spots);

        public bool Add(params ParkingSpot[] spots)
        {
            var mergeSpots = new List<ParkingSpot>(_spots);
            mergeSpots.AddRange(spots);

            var duplicates = mergeSpots.GroupBy(x => x)
                .Where(y => y.Count() > 1)
                .Select(z => z.GetHashCode());

            if (duplicates.Any())
            {
                return false;
            }

            _spots = mergeSpots;
            return true;
        }

        #region IEnumerator
        public IEnumerator<ParkingSpot> GetEnumerator()
        {
            return _spots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}