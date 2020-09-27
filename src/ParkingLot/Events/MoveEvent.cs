using System;
using System.Diagnostics.CodeAnalysis;

namespace IntrepidProducts.ParkingLot.Events
{
    public class MoveEvent : ParkedEvent
    {
        public MoveEvent([DisallowNull] ParkingSpot spot, DateTime? evenTime = null)
            : base(spot, evenTime)
        {
            EventType = OccupiedEventType.Moved;
        }
    }
}