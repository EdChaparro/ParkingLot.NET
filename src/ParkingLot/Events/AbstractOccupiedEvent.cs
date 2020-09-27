using System;

namespace IntrepidProducts.ParkingLot.Events
{
    public enum OccupiedEventType
    {
        Entered = 1,
        Parked = 2,
        Exited = 3,
        Moved = 4
    }

    public interface IOccupiedEvent
    {
        Guid Id { get; }
        OccupiedEventType EventType { get; }
        DateTime EvenTime { get; }
    }

    public abstract class AbstractOccupiedEvent : IOccupiedEvent
    {
        protected AbstractOccupiedEvent(
            OccupiedEventType eventType,
            DateTime? evenTime = null)
        {
            Id = Guid.NewGuid();
            EventType = eventType;
            EvenTime = evenTime ?? DateTime.Now;
        }
        public Guid Id { get; }
        public OccupiedEventType EventType { get; protected set; }

        public DateTime EvenTime { get; }
    }
}