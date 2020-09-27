using IntrepidProducts.ParkingLot.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.ParkingLot.Tests
{
    [TestClass]
    public class OccupiedSessionTest
    {
        [TestMethod]
        public void ShouldRequireFirstEventIsEntered()
        {
            var session = new OccupiedSession();
            Assert.IsFalse(session.Add(new ParkedEvent(new ParkingSpot("1"))));
            Assert.IsFalse(session.Add(new ExitedEvent()));
            Assert.IsFalse(session.Add(new MoveEvent(new ParkingSpot("2"))));

            var vehicle = new Vehicle("Lexus", "XXX");
            Assert.IsTrue(session.Add(new EnteredEvent(), vehicle));

            Assert.AreEqual(1, session.Events.Count);
            Assert.AreEqual(vehicle, session.Vehicle);
        }

        [TestMethod]
        public void ShouldOnlyPermitOneEnteredEvent()
        {
            var session = new OccupiedSession();
            var vehicle = new Vehicle("Lexus", "XXX");

            Assert.IsTrue(session.Add(new EnteredEvent(), vehicle));
            Assert.IsFalse(session.Add(new EnteredEvent(), vehicle));

            Assert.AreEqual(1, session.Events.Count);
        }

        [TestMethod]
        public void ShouldOnlyPermitOneExitedEvent()
        {
            var session = new OccupiedSession();
            var vehicle = new Vehicle("Lexus", "XXX");

            Assert.IsTrue(session.Add(new EnteredEvent(), vehicle));
            Assert.IsTrue(session.Add(new ExitedEvent()));
            Assert.IsFalse(session.Add(new ExitedEvent()));

            Assert.AreEqual(2, session.Events.Count);
        }

        [TestMethod]
        public void ShouldOnlyPermitMoveOnceParked()
        {
            var session = new OccupiedSession();
            var vehicle = new Vehicle("Lexus", "XXX");

            Assert.IsTrue(session.Add(new EnteredEvent(), vehicle));
            Assert.IsFalse(session.Add(new MoveEvent(new ParkingSpot("1"))));
            Assert.IsTrue(session.Add(new ParkedEvent(new ParkingSpot("1"))));
            Assert.IsTrue(session.Add(new MoveEvent(new ParkingSpot("2"))));
            Assert.AreEqual(3, session.Events.Count);

            Assert.IsTrue(session.Add(new MoveEvent(new ParkingSpot("3"))));
            Assert.AreEqual(4, session.Events.Count);

            Assert.AreEqual(new ParkingSpot("3"), session.ParkingSpot);
        }

        [TestMethod]
        public void ShouldOnlyPermitMoveWhenDifferentParkingSpotIsAssigned()
        {
            var session = new OccupiedSession();
            var vehicle = new Vehicle("Lexus", "XXX");

            Assert.IsTrue(session.Add(new EnteredEvent(), vehicle));
            Assert.IsTrue(session.Add(new ParkedEvent(new ParkingSpot("1"))));
            Assert.IsFalse(session.Add(new MoveEvent(new ParkingSpot("1"))));
            Assert.AreEqual(2, session.Events.Count);

            Assert.IsTrue(session.Add(new MoveEvent(new ParkingSpot("3"))));
            Assert.AreEqual(3, session.Events.Count);

            Assert.IsTrue(session.Add(new MoveEvent(new ParkingSpot("1"))));
            Assert.IsFalse(session.Add(new MoveEvent(new ParkingSpot("1"))));

            Assert.AreEqual(4, session.Events.Count);
        }

        [TestMethod]
        public void ShouldAssignParkingSpot()
        {
            var session = new OccupiedSession();
            var vehicle = new Vehicle("Lexus", "XXX");

            Assert.IsTrue(session.Add(new EnteredEvent(), vehicle));

            var spot = new ParkingSpot("1");
            Assert.IsTrue(session.Add(new ParkedEvent(spot)));

            var lastEvent = session.LastEvent as ParkedEvent;
            Assert.IsNotNull(lastEvent);

            Assert.AreEqual(lastEvent.ParkingSpot, spot);
        }
    }
}