using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using IntrepidProducts.ParkingLot.Events;

namespace IntrepidProducts.ParkingLot
{
    public interface IParkingManager
    {
        int TotalNbrOfSpots { get; }
        int TotalNbrOfAvailableSpots { get; }

        bool IsFull { get; }

        ParkingSpot? AvailableSpot();

        bool IsAvailable(ParkingSpot spot);

        bool Enter(IVehicle vehicle);
        bool Exit(IVehicle vehicle, DateTime? dateTime = null);
        
        bool Park(IVehicle vehicle, ParkingSpot spot = null);
        bool Move(IVehicle vehicle, ParkingSpot spot = null);

        ParkingSpot? FindParkingSpot(IVehicle vehicle);
    }

    public class ParkingManager : IParkingManager
    {
        public ParkingManager(IParkingFacility facility)
        {
            _facility = facility;
        }

        private readonly IParkingFacility _facility;

        private readonly List<OccupiedSession> _activeSessions
            = new List<OccupiedSession>();

        private readonly List<OccupiedSession> _inactiveSessions  //TODO: Use it or lost it
            = new List<OccupiedSession>();

        public int TotalNbrOfSpots => _facility.All.Count;
        public int TotalNbrOfAvailableSpots => TotalNbrOfSpots - _activeSessions.Count;
        public bool IsFull => !(_activeSessions.Count < TotalNbrOfSpots);

        public ParkingSpot? FindParkingSpot(IVehicle vehicle)
        {
            var session = _activeSessions
                .FirstOrDefault(x => x.Vehicle != null && x.Vehicle.Equals(vehicle));

            return session?.ParkingSpot;
        }

        public ParkingSpot? AvailableSpot()
        {
            var activeParkingSpots =
                _activeSessions
                    .Where(x => x.ParkingSpot != null)
                    .Select(x => x.ParkingSpot);

            var parkingSpots = activeParkingSpots.ToList();

            return _facility.All.Except(parkingSpots).FirstOrDefault();
        }

        public bool IsAvailable([NotNull] ParkingSpot spot)
        {
            var isAnActualSpot = _facility.All.Contains(spot);

            if (!isAnActualSpot)
            {
                return false;
            }

            var isActive = _activeSessions
                .Where(x => x.ParkingSpot != null)
                .Any(x => x.ParkingSpot != null && x.ParkingSpot.Equals(spot));

            return !isActive;
        }

        private bool IsVehicleActive(IVehicle vehicle)
        {
            return _activeSessions
                .Any(x => x.Vehicle == vehicle);
        }

        public bool Enter(IVehicle vehicle)
        {
            if (IsVehicleActive(vehicle))
            {
                return false;
            }

            if (IsFull)
            {
                return false;
            }

            var session = new OccupiedSession();
            session.Add(new EnteredEvent(), vehicle);
            _activeSessions.Add(session);

            return true;
        }

        private OccupiedSession FindActiveSession(IVehicle vehicle) =>
            _activeSessions
                .FirstOrDefault(x => 
                    x.Vehicle != null && x.Vehicle.Equals(vehicle));

        public bool Exit(IVehicle vehicle, DateTime? dateTime = null)
        {
            var session = FindActiveSession(vehicle);

            if (session == null)
            {
                return false;
            }

            var exitDateTime = dateTime ?? DateTime.Now;

            bool isAdded = session.Add(new ExitedEvent(exitDateTime));

            if (!isAdded)
            {
                return false;
            }

            var isRemoved = _activeSessions.Remove(session);

            if (isRemoved)
            {
                _inactiveSessions.Add(session);
            }

            return isRemoved;
        }

        public bool Park(IVehicle vehicle, ParkingSpot spot = null)
        {
            var session = FindActiveSession(vehicle);

            if (session == null)
            {
                return false;
            }

            var parkingSpot = spot ?? AvailableSpot();

            if (parkingSpot == null)
            {
                return false;
            }

            return session.Add(new ParkedEvent(parkingSpot));
        }

        public bool Move(IVehicle vehicle, ParkingSpot spot = null)
        {
            var session = FindActiveSession(vehicle);

            if (session == null)
            {
                return false;
            }

            var parkingSpot = spot ?? AvailableSpot();

            if (parkingSpot == null)
            {
                return false;
            }

            return session.Add(new MoveEvent(parkingSpot));
        }
    }
}