using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        public decimal CurrentMoney { get; set; }

        public List<VendingMachineItem> items = new List<VendingMachineItem>(); //needs populated when file is uploaded
        public VendingMachine()
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
            bool validTender = insertedMoney == 1 || insertedMoney == 2 || insertedMoney == 5 || insertedMoney == 10;
            if (validTender)
            {
                CurrentMoney += insertedMoney;
            }
        }
        public string BuyItem(string slot)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Slot == slot)
                {
                    if (items[i].Quantity != 0)
                    {
                        CurrentMoney -= items[i].Price;
                        return items[i].Consume();
                    }
                    return "SOLD OUT";
                }
            }
            return "Product code does not exist";
        }
    }
    //public void CoinOut()
    //{
    //    Console.WriteLine();
    //}
}

