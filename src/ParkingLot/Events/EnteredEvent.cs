using System;

namespace IntrepidProducts.ParkingLot.Events
{
    public class EnteredEvent : AbstractOccupiedEvent
    {
        public EnteredEvent(DateTime? evenTime = null)
            : base(OccupiedEventType.Entered, evenTime)
        {}

        public static EnteredEvent Cast(IOccupiedEvent occupiedEvent)
        {
            return occupiedEvent as EnteredEvent;
        }
    }
}