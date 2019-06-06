using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachineItem
    {


        public string Slot {get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }


        public string Consume()
        {
            if (Slot.Substring(0, 1) = "A")
            {
                Quantity -= 1;
                return "Crunch Crunch, Yum!";
            }

            else if (Slot.Substring(0, 1) = "B")
            {
                Quantity -= 1;
                return "Munch Munch, Yum!";
            }

            else if (Slot.Substring(0, 1) = "C")
            {
                Quantity -= 1;
                return "Glug Glug, Yum!";
            }

            else if (Slot.Substring(0, 1) = "D")
            {
                Quantity -= 1;
                return "Chew Chew, Yum!";
            }

            else
            {
                return "Invalid Item";
            }
        }


        //A1|Potato Crisps|3.05
        //A2|Stackers|1.45
        //A3|Grain Waves|2.75
        //A4|Cloud Popcorn|3.65
        //B1|Moonpie|1.80
        //B2|Cowtales|1.50
        //B3|Wonka Bar|1.50
        //B4|Crunchie|1.75
        //C1|Cola|1.25
        //C2|Dr.Salt|1.50
        //C3|Mountain Melter|1.50
        //C4|Heavy|1.50
        //D1|U-Chews|0.85
        //D2|Little League Chew|0.95
        //D3|Chiclets|0.75
        //D4|Triplemint|0.75
    }
}
