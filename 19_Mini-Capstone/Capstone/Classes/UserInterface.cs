using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class UserInterface
    {

        private VendingMachine vendingMachine = new VendingMachine();


        public void RunInterface()
        {
            bool done = false;
            while (!done)
            {
                // display main menu options
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");
                Console.WriteLine("(3) End");
                string mainMenuOption = Console.ReadLine();
                string purchaseMenuOption = "";

                // Main menu cases
                switch (mainMenuOption)
                {
                    case "1":
                        // display ItemName Price Quantity 
                        foreach (VendingMachineItem item in vendingMachine.items)
                        {
                            Console.WriteLine($"{item.Slot} {item.ProductName} ${item.Price} {item.Quantity}");
                        }
                        break;

                    case "2":
                        bool purchaseDone = false;
                        while (!purchaseDone)
                        {
                            // display purchase menu options
                            Console.WriteLine("(1) Feed Money");
                            Console.WriteLine("(2) Select Product");
                            Console.WriteLine("(3) Finish Transaction");
                            Console.WriteLine($"Current Money Provided: {vendingMachine.CurrentMoney}");
                            purchaseMenuOption = Console.ReadLine();
                            // Purchase menu cases
                            switch (purchaseMenuOption)
                            {
                                case "1":
                                    // feed money
                                    Console.Write("Enter dollar amount to insert: ");
                                    int feeding = int.Parse(Console.ReadLine());
                                    vendingMachine.FeedMoney(feeding);
                                    break;

                                case "2":
                                    // select product
                                    foreach (VendingMachineItem item in vendingMachine.items)
                                    {
                                        Console.WriteLine($"{item.Slot} {item.ProductName} ${item.Price} {item.Quantity}");
                                    }
                                    Console.Write("Enter an item slot code: ");
                                    string desiredSlot = Console.ReadLine();
                                    Console.Write(vendingMachine.BuyItem(desiredSlot));
                                    Console.ReadLine();
                                    break;

                                case "3":
                                    // finish transaction
                                    //vendingMachine.CoinOut();
                                    Console.Write("Please take your change.");
                                    Console.ReadLine();
                                    purchaseDone = true;
                                    break;

                                default:
                                    break;
                            }
                            Console.Clear();
                        }
                        break;

                    case "3":
                        // end
                        done = true;
                        break;

                    default:
                        break;
                }
                Console.ReadLine();
            }
        }

    }
}

