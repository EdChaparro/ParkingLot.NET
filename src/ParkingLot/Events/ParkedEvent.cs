using System;
using System.Diagnostics.CodeAnalysis;

namespace IntrepidProducts.ParkingLot.Events
{
    public interface IParkingEvent : IOccupiedEvent
    {
        ParkingSpot ParkingSpot { get; }
    }

    public class ParkedEvent : AbstractOccupiedEvent, IParkingEvent
    {
        public ParkedEvent([DisallowNull] ParkingSpot spot, DateTime? evenTime = null) 
            : base(OccupiedEventType.Parked, evenTime)
        {
            ParkingSpot = spot;
        }
        public ParkingSpot ParkingSpot { get; }
    }
}