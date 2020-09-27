using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.ParkingLot.Tests
{
    [TestClass]
    public class ParkingManagerTest
    {
        public ParkingManagerTest()
        {
            _garage.Add(_spot1, _spot2, _spot3);
        }

        private readonly ParkingFacility _garage = new ParkingFacility();
        private readonly ParkingSpot _spot1 = new ParkingSpot("1");
        private readonly ParkingSpot _spot2 = new ParkingSpot("2");
        private readonly ParkingSpot _spot3 = new ParkingSpot("3");

        private readonly IVehicle _vehicle1 = new Vehicle("Lexus", "123");
        private readonly IVehicle _vehicle2 = new Vehicle("Lexus", "456");
        private readonly IVehicle _vehicle3 = new Vehicle("Lexus", "789");

        private ParkingManager _manager;

        [TestInitialize]
        public void Initialized()
        {
            _manager = new ParkingManager(_garage);
        }

        [TestMethod]
        public void ShouldReportAvailableSpots()
        {
            Assert.AreEqual(3, _manager.TotalNbrOfSpots);
        }

        [TestMethod]
        public void ShouldReportWhenFull()
        {
            Assert.IsFalse(_manager.IsFull);

            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.IsTrue(_manager.Enter(_vehicle2));
            Assert.IsTrue(_manager.Enter(_vehicle3));

            Assert.IsTrue(_manager.IsFull);
        }

        [TestMethod]
        public void ShouldReportWhichSpotsAreAvailable()
        {
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.IsTrue(_manager.Enter(_vehicle2));
            Assert.IsTrue(_manager.Enter(_vehicle3));

            Assert.AreEqual(_spot1, _manager.AvailableSpot());
            _manager.Park(_vehicle1, _spot1);

            Assert.AreEqual(_spot2, _manager.AvailableSpot());
            _manager.Park(_vehicle2, _spot2);

            Assert.AreEqual(_spot3, _manager.AvailableSpot());
            _manager.Park(_vehicle3, _spot3);

            Assert.IsNull(_manager.AvailableSpot());
        }

        [TestMethod]
        public void ShouldReportWhenSpotIsAvailable()
        {
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.IsTrue(_manager.Enter(_vehicle2));
            Assert.IsTrue(_manager.Enter(_vehicle3));

            Assert.IsTrue(_manager.IsAvailable(_spot1));
            _manager.Park(_vehicle1, _spot1);
            Assert.IsFalse(_manager.IsAvailable(_spot1));

            Assert.IsTrue(_manager.IsAvailable(_spot2));
            _manager.Park(_vehicle2, _spot2);
            Assert.IsFalse(_manager.IsAvailable(_spot2));

            Assert.IsTrue(_manager.IsAvailable(_spot3));
            _manager.Park(_vehicle3, _spot3);
            Assert.IsFalse(_manager.IsAvailable(_spot3));
        }
        [TestMethod]
        public void ShouldReduceAvailableSpotsOnEnterEvent()
        {
            Assert.AreEqual(3, _manager.TotalNbrOfAvailableSpots);
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.AreEqual(2, _manager.TotalNbrOfAvailableSpots);
        }

        [TestMethod]
        public void ShouldNotAcceptEnterEventWhenVehicleEntryAlreadyActive()
        {
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.IsFalse(_manager.Enter(_vehicle1));
            Assert.IsTrue(_manager.Enter(_vehicle2));
        }

        [TestMethod]
        public void ShouldIncreaseAvailableSpotsOnExit()
        {
            Assert.AreEqual(3, _manager.TotalNbrOfAvailableSpots);
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.AreEqual(2, _manager.TotalNbrOfAvailableSpots);

            Assert.IsTrue(_manager.Exit(_vehicle1));
            Assert.AreEqual(3, _manager.TotalNbrOfAvailableSpots);
        }

        [TestMethod]
        public void ShouldFindVehicleParkingSpot()
        {
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.IsTrue(_manager.Park(_vehicle1, _spot1));

            Assert.IsTrue(_manager.Enter(_vehicle2));
            Assert.IsTrue(_manager.Park(_vehicle2, _spot2));

            Assert.AreEqual(_spot1, _manager.FindParkingSpot(_vehicle1));
            Assert.AreEqual(_spot2, _manager.FindParkingSpot(_vehicle2));
        }

        [TestMethod]
        public void ShouldRegisterNewParkingSpotOnMove()
        {
            Assert.AreEqual(3, _manager.TotalNbrOfAvailableSpots);
            Assert.IsTrue(_manager.Enter(_vehicle1));
            Assert.IsTrue(_manager.Park(_vehicle1, _spot1));
            Assert.AreEqual(2, _manager.TotalNbrOfAvailableSpots);
            Assert.AreEqual(_spot1, _manager.FindParkingSpot(_vehicle1));

            Assert.IsTrue(_manager.Move(_vehicle1, _spot2));
            Assert.AreEqual(_spot2, _manager.FindParkingSpot(_vehicle1));
            Assert.AreEqual(2, _manager.TotalNbrOfAvailableSpots);
        }
    }
}