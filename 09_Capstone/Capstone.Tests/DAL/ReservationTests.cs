using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class ReservationTests
    {
        Reservation reservation;

        [TestInitialize]
        public void Setup()
        {
            reservation = new Reservation
            {

                Reservation_Id = 1,
                Site_Id = 2,
                Name = "Name",
                From_Date = DateTime.Parse("2000-01-01"),
                To_Date = DateTime.Parse("2000-01-02"),
                Cost = 10.00M
            };
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("01/01/2000     01/02/2000     2         1              Name                          ", reservation.ToString());
        }

        [TestMethod]
        public void ReservationListHeaderTest()
        {
            Assert.AreEqual("From Date      To Date        Site ID   Reservation ID Name", Reservation.ReservationListHeader());
        }
    }
}
