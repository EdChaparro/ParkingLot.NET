using System;

namespace IntrepidProducts.ParkingLot.Events
{
    public class ExitedEvent : AbstractOccupiedEvent
    {
        public ExitedEvent(DateTime? evenTime = null) 
            : base(OccupiedEventType.Exited, evenTime)
        { }
    }
}