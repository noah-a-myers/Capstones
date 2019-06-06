using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachine
    {

        private List<VendingMachineItem> items = new List<VendingMachineItem>(); //needs populated when file is uploaded

        private void FillMachine()
        {
            string filePath = @"C:\VendingMachine\vendingmachine.csv";
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    VendingMachineItem newItem = new VendingMachineItem();
                    string line = sr.ReadLine();
                    string[] props = line.Split('|');
                    newItem.Slot = props[0];
                    newItem.ProductName = props[1];
                    newItem.Price = decimal.Parse(props[2]);
                    items.Add(newItem);
                }
            }
            
        }
        public void FeedMoney(int insertedMoney)
        {

        }
        public void BuyItem(string slot)
        {

        }
        public void CoinOut()
        {

        }
    }
}
