using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.ParkingLot.Tests
{
    [TestClass]
    public class ParkingFacilityTest
    {
        [TestMethod]
        public void ShouldOnlyAcceptUniqueParkingSpots()
        {
            var garage = new ParkingFacility();
            Assert.AreEqual(0, garage.All.Count);

            var spot1 = new ParkingSpot("1");
            var spot2 = new ParkingSpot("2");
            var spot3 = new ParkingSpot("3");

            Assert.IsTrue(garage.Add(spot1, spot2, spot3));
            Assert.AreEqual(3, garage.All.Count);

            Assert.IsFalse(garage.Add(spot3));
            Assert.AreEqual(3, garage.All.Count);
        }

        [TestMethod]
        public void ShouldSupportParkingSpotCollectionInitializer()
        {
            var spot1 = new ParkingSpot("1");
            var spot2 = new ParkingSpot("2");
            var spot3 = new ParkingSpot("3");

            var garage = new ParkingFacility {spot1, spot2, spot3};

            Assert.AreEqual(3, garage.All.Count);

            Assert.IsFalse(new List<ParkingSpot> {spot1, spot2, spot3 }.Except(garage.All).Any());
        }
    }
}