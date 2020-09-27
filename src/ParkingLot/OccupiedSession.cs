using System.Collections.Generic;
using System.Linq;
using IntrepidProducts.ParkingLot.Events;

namespace IntrepidProducts.ParkingLot
{
    public class OccupiedSession 
    {
        public IList<IOccupiedEvent> Events { get; } = new List<IOccupiedEvent>();

        public IVehicle? Vehicle { get; private set; }

        #region Add Events
        public bool Add(EnteredEvent oEvent, IVehicle vehicle)
        {
            Vehicle = vehicle;

            if (!HasEntered)
            {
                Events.Add(oEvent);
                return true;
            }

            return false;
        }

        public bool Add(ExitedEvent oEvent)
        {
            if (HasEntered && !HasExited)
            {
                Events.Add(oEvent);
                return true;
            }

            return false;
        }

        public bool Add(ParkedEvent oEvent)
        {
            if (HasEntered && !IsParked)
            {
                Events.Add(oEvent);
                return true;
            }

            return false;
        }

        public bool Add(MoveEvent oEvent)
        {
            if (IsParked)
            {
                var lastEvent = LastEvent as ParkedEvent;

                if (lastEvent == null)
                {
                    return false;
                }

                if (lastEvent.ParkingSpot.Equals(oEvent.ParkingSpot))
                {
                    return false;
                }

                Events.Add(oEvent);
                return true;
            }

            return false;
        }
        #endregion  

        public IOccupiedEvent LastEvent
        {
            get => Events.LastOrDefault();
        }

        public ParkingSpot? ParkingSpot
        {
            get
            {
                var lastEvent = LastEvent as ParkedEvent;
                return lastEvent?.ParkingSpot;
            }
        }

        public bool IsStillPresent => !HasExited;

        private bool HasEntered
        {
            get
            {
                return Events.Any(x =>
                    x.EventType == OccupiedEventType.Entered);
            }
        }

        public bool HasExited
        {
            get
            {
                return Events.Any(x => 
                    x.EventType == OccupiedEventType.Exited);
            }
        }

        public bool IsParked
        {
            get
            {
                return !HasExited && Events.Any(x =>
                    x.EventType == OccupiedEventType.Parked);
            }
        }
    }
}