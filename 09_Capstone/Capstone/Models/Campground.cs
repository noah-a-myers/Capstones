using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int Campground_Id { get; set; } // Identity
        public int Park_Id { get; set; }
        public string Name { get; set; }
        public string Open_From_MM { get; set; }
        public string Open_To_MM { get; set; }
        public decimal Daily_Fee { get; set; }

        public override string ToString()
        {
            return "#" + Campground_Id.ToString().PadRight(5) + Name.PadRight(35) + Open_From_MM.PadRight(15) + Open_To_MM.PadRight(15) + "$" + string.Format("{0:0.00}", Daily_Fee);
        }

        public static string PrintCampgroundHeader()
        {
            return "      Name".PadRight(41) + "Open".PadRight(15) + "Close".PadRight(15) + "Daily Fee";
        }
    }
}
