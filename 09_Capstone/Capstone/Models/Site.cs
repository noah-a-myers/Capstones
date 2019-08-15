using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int Site_Id { get; set; } // Identity
        public int Campground_Id { get; set; }
        public string Campground_Name { get; set; }
        public int Site_Number { get; set; }
        public int Max_Occupancy { get; set; }
        public bool Accessible { get; set; }
        public int Max_RV_Length { get; set; }
        public bool Utilities { get; set; }
        public decimal DailyFee { get; set; }

        public override string ToString()
        {
            return Site_Number.ToString().PadRight(15) + Max_Occupancy.ToString().PadRight(15) + (Accessible?"Yes":"No").PadRight(20) + (Max_RV_Length == 0?"N/A":Max_RV_Length.ToString()).PadRight(20) + (Utilities?"Yes":"N/A").PadRight(15);
        }

        public string ToStringWithName()
        {
            return Campground_Name.PadRight(35) + Site_Number.ToString().PadRight(15) + Max_Occupancy.ToString().PadRight(15) + (Accessible ? "Yes" : "No").PadRight(20) + (Max_RV_Length == 0 ? "N/A" : Max_RV_Length.ToString()).PadRight(20) + (Utilities ? "Yes" : "N/A").PadRight(15);
        }

        public static string PrintSiteHeaders()
        {
            return "Site No.".PadRight(15) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(20) + "Max RV Length".PadRight(20) + "Utility".PadRight(15) + "Cost";
        }

        public static string PrintSiteHeadersWithName()
        {
            return "Campground".PadRight(35) + "Site No.".PadRight(15) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(20) + "Max RV Length".PadRight(20) + "Utility".PadRight(15) + "Cost";
        }
    }
}
