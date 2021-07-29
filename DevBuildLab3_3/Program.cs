using System;
using System.Collections.Generic;

namespace DevBuildLab3_3
{
    class MenuItem
    {
        public string Name;
        public decimal Price;
        public int Quantity;
        public void Sell(int numberSold)
        {
            Quantity = Quantity - numberSold;
        }
    }

    class Program
    {
        
        static void PrintMenu(Dictionary<string, MenuItem> keyValuePairs) //Method to display the menu.
        {
            Console.WriteLine("---------------------");
            Console.WriteLine(" ~  CURRENT MENU  ~ ");
            Console.WriteLine("---------------------");
            foreach (var item in keyValuePairs)
            {
                Console.Write(item.Value.Name.PadRight(17));
                Console.Write(string.Format($"{item.Value.Price:0.00}  "));
                if (item.Value.Quantity < 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"({item.Value.Quantity.ToString().PadRight(2)} on hand) - order more!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"({item.Value.Quantity.ToString().PadRight(2)} on hand)");
                }
            }
        }
        
        static void ShowOptions() //Method to display options. Created mostly so I could get it out of sight after playing with colors.
        {
            Console.Write("\nPress: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("S");
            Console.ResetColor();
            Console.Write(" - log a sale | ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("A");
            Console.ResetColor();
            Console.Write(" - add an item | ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("R");
            Console.ResetColor();
            Console.Write(" - remove an item | ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("C");
            Console.ResetColor();
            Console.Write(" - change a price | ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("V");
            Console.ResetColor();
            Console.Write(" - view menu | ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Q");
            Console.ResetColor();
            Console.WriteLine(" - quit");
        }

        static void Main(string[] args)
        {
            MenuItem menuItem1 = new MenuItem();
            menuItem1.Name = "Nachos";
            menuItem1.Price = 4.99m;
            menuItem1.Quantity = 20;

            MenuItem menuItem2 = new MenuItem();
            menuItem2.Name = "Hot Dog";
            menuItem2.Price = 3.99m;
            menuItem2.Quantity = 8;

            MenuItem menuItem3 = new MenuItem();
            menuItem3.Name = "Hamburger";
            menuItem3.Price = 6.99m;
            menuItem3.Quantity = 10;

            MenuItem menuItem4 = new MenuItem();
            menuItem4.Name = "Popcorn";
            menuItem4.Price = 3.99m;
            menuItem4.Quantity = 2;

            Dictionary<string, MenuItem> menu = new Dictionary<string, MenuItem>();
            menu["item1"] = menuItem1;
            menu["item2"] = menuItem2;
            menu["item3"] = menuItem3;
            menu["item4"] = menuItem4;

            int itemKeyCounter = 4; //To facilitate adding items

            PrintMenu(menu);

            bool quit = false;
            while (!quit)
            {
                ShowOptions();
                bool itemAlreadyExists = false;

                string input = Console.ReadLine().ToUpper();
                if (input == "S")
                {
                    Console.Write("Enter the item being sold: (or type CANCEL) ");
                    string soldItem = Console.ReadLine();
                    if (soldItem.ToUpper().Trim() != "CANCEL")
                    {
                        foreach (var item in menu)   //Check if item already exists
                        {
                            if (item.Value.Name.ToLower() == soldItem.ToLower().Trim()) 
                            {
                                Console.Write("Enter the quantity to sell: ");
                                int.TryParse(Console.ReadLine(), out int qtyToSell);   //force valid qty to sell. I considered adding the CANCEL here since 0 is invalid but decided to move on.
                                while (qtyToSell == 0)
                                {
                                    Console.Write("Please enter a valid quantity: ");
                                    int.TryParse(Console.ReadLine(), out qtyToSell);
                                }
                                if (qtyToSell > item.Value.Quantity)      //Cannot sell more than on-hand
                                {
                                    Console.WriteLine($"There are not enough {item.Value.Name}'s in stock. Only {item.Value.Quantity} can be sold at this time.");
                                    Console.Write($"Sell {item.Value.Quantity}? (Y/N) ");   //Offer to sell whats left.
                                    if (Console.ReadLine().ToLower().StartsWith("y"))
                                    {
                                        qtyToSell = item.Value.Quantity;
                                        item.Value.Quantity = item.Value.Quantity - qtyToSell;
                                        Console.WriteLine($"Sold {qtyToSell} {item.Value.Name}.");
                                    }
                                }
                                else
                                {
                                    item.Value.Sell(qtyToSell);        //use the method from our class 
                                    Console.WriteLine($"Sold {qtyToSell} {item.Value.Name}."); 
                                }
                                itemAlreadyExists = true;
                            }
                        }
                        if (itemAlreadyExists == false)
                        {
                            Console.WriteLine("Item was not found. Please try again.");
                        }
                    }
                }
                else if (input == "A")
                {
                    Console.Write("Please enter the name of the new item: (or type CANCEL) ");
                    string newItem = Console.ReadLine();
                    if (newItem.ToUpper().Trim() != "CANCEL")
                    {
                        foreach (var item in menu) //CheckForItem   //Try the ContainsKey method another time...
                        {
                            if (item.Value.Name.ToLower() == newItem.ToLower().Trim())
                            {
                                Console.WriteLine("Item was already found on the menu.");
                                itemAlreadyExists = true;      //We will skip the next Add logic if the item already exists
                            }
                        }
                        if (itemAlreadyExists == false)
                        {
                            MenuItem menuItem = new MenuItem();                      //create new object
                            Console.Write($"Please enter the price of {newItem}: ");
                            decimal.TryParse(Console.ReadLine(), out decimal newItemPrice);
                            while (newItemPrice == 0)                                //force valid price
                            {
                                Console.Write("Invalid price. Please re-enter: ");
                                decimal.TryParse(Console.ReadLine(), out newItemPrice);
                            }

                            Console.Write("Please enter the quantity added: ");
                            int.TryParse(Console.ReadLine(), out int newItemQty);
                            while (newItemQty == 0)                                   //force valid qty
                            {
                                Console.Write("Invalid quantity. Please re-enter: ");
                                int.TryParse(Console.ReadLine(), out  newItemQty);
                            }

                            menuItem.Name = newItem;
                            menuItem.Price = newItemPrice;
                            menuItem.Quantity = newItemQty;
                            itemKeyCounter++;             
                            menu["item" + itemKeyCounter] = menuItem;    //use counter to create new unique key
                            Console.WriteLine($"{newItem} added!");
                        }
                    }
                }
                else if (input == "R")
                {
                    Console.Write("Please enter the name of the item to be removed: (or type CANCEL) ");
                    string removeItem = Console.ReadLine();
                    if (removeItem.ToUpper().Trim() != "CANCEL")
                    {
                        foreach (var item in menu)    //Check for item
                        {
                            if (item.Value.Name.ToLower() == removeItem.ToLower().Trim())
                            {
                                removeItem = item.Value.Name; //correct casing differences (this was needed in the first (no classes) version of the lab since it was used as the key to remove)
                                itemAlreadyExists = true;
                                menu.Remove(item.Key);
                                Console.WriteLine($"{removeItem} removed.");
                            }
                        }
                        if (itemAlreadyExists == false)
                        {
                            Console.WriteLine("Item was not found. Please try again.");
                        }
                    }
                }
                else if (input == "C")
                {
                    Console.Write("Please enter the name of the item: (or type CANCEL) ");
                    string changeItem = Console.ReadLine();
                    if (changeItem.ToUpper().Trim() != "CANCEL")
                    {
                        //Check for item
                        string keyToChange = "";
                        foreach (var item in menu)
                        {
                            if (item.Value.Name.ToLower() == changeItem.ToLower().Trim())
                            {
                                changeItem = item.Value.Name; //correct for casing differences
                                keyToChange = item.Key;
                                itemAlreadyExists = true;
                            }
                        }
                        if (itemAlreadyExists)
                        {
                            Console.Write($"\n**{changeItem} is currently priced at {menu[keyToChange].Price}** \nEnter the new price: (or type CANCEL) ");
                            string enteredPrice = Console.ReadLine();
                            if (enteredPrice.ToUpper().Trim() != "CANCEL")
                            {
                                Decimal.TryParse(enteredPrice, out decimal newPrice);
                                while (newPrice == 0)
                                {
                                    Console.Write("Invalid price. Please re-enter: ");
                                    Decimal.TryParse(Console.ReadLine(), out newPrice);
                                }
                                Console.WriteLine($"{changeItem} changed from {menu[keyToChange].Price} to {newPrice}");
                                menu[keyToChange].Price = newPrice;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Item was not found. Please try again.");
                        }
                    }
                }
                else if (input == "V")
                {
                    PrintMenu(menu);
                }
                else if (input == "Q")
                {
                    Console.WriteLine("\nGoodbye!\n");
                    quit = true;
                }
                else if (input == "KEYS") //Hidden command to see that keys are generating as intended. I wasn't sure how that is supposed to be handled.
                {
                    foreach (KeyValuePair<string, MenuItem> key in menu)
                    {
                        Console.WriteLine($"{key.Key}: {key.Value.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection" + "\nPlease enter A, R, C, V, or Q");
                }
            }

        }
    }
}
