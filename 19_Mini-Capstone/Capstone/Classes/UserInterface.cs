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
                        // todo display ItemName Price Quantity 
                        Console.WriteLine($"");
                        break;

                    case "2":
                        // display purchase menu options
                        Console.WriteLine("(1) Feed Money");
                        Console.WriteLine("(2) Select Product");
                        Console.WriteLine("(3) Finish Transaction");
                        Console.WriteLine("Current Money Provided: {classContainingMethod.CurrentMoneyProvided}");
                        purchaseMenuOption = Console.ReadLine();
                        break;

                    case "3":
                        // end
                        done = true;
                        break;

                    default:
                        break;
                }

                

                // if case 2 selected in main menu
                if (mainMenuOption == "2")
                {
                    bool purchaseDone = false;
                    while (!purchaseDone)
                    {
                        // Purchase menu cases
                        switch (purchaseMenuOption)
                        {
                            case "1":
                                // feed money
                                Console.WriteLine("classContainingMethod.FeedMoney()");
                                break;

                            case "2":
                                // select product
                                Console.WriteLine("classContainingMethod.SelectProduct()");
                                break;

                            case "3":
                                // finish transaction
                                Console.WriteLine("classContainingMethod.FinishTransaction()");
                                purchaseDone = true;
                                break;

                            default:
                                break;
                        }
                    }
                }

                

                Console.ReadLine();

            }
        }
    }
}
