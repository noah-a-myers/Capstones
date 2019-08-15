using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Reservation_Id { get; set; } // Identity
        public int Site_Id { get; set; }
        public string Name { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public decimal Cost { get; set; }

        public override string ToString()
        {
            return From_Date.ToString("MM/dd/yyy").PadRight(15) + To_Date.ToString("MM/dd/yyy").PadRight(15) + Site_Id.ToString().PadRight(10) + Reservation_Id.ToString().PadRight(15) + Name.PadRight(30);
        }

        public static string ReservationListHeader()
        {
            return "From Date".PadRight(15) + "To Date".PadRight(15) + "Site ID".PadRight(10) + "Reservation ID".PadRight(15) + "Name";
        }
    }
}
