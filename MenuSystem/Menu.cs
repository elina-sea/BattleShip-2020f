using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public enum MenuLevel
    {
        FirstWhetherReady, //0
        SecondGameMode, //1
        ShipsToPlace, //2
        PassOrExit
    }

    public class Menu
    {
        public string UserChoice = null!;
        private static string EXIT_CODE = "x";
        //private static string CONFIRM_CODE = "c";
        private Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();
        private readonly MenuLevel _menuLevel;

        public Menu(MenuLevel menuLevel)
        {
            _menuLevel = menuLevel; 
        }

        public void AddMenuItem(MenuItem item)
        {
            if (item.OptionLabelToChoose == "")
            {
                throw new Exception("User choice cannot be empty");
            }
            
            MenuItems.Add(item.OptionLabelToChoose, item);
        }

        public void RunMenu()
        {
            var userChoice = "";
            Console.WriteLine("Menu cycle is runned");
           
            while (userChoice != EXIT_CODE) {
                //displaying the menu
                foreach (var menuItem in MenuItems)
                {
                    Console.WriteLine(menuItem.Value); //ToString is overriden so we see a nice output here
                }

                switch (_menuLevel)
                {
                    case MenuLevel.FirstWhetherReady:
                        Console.WriteLine("x) eXit");
                        break;
                    case MenuLevel.SecondGameMode:
                        Console.WriteLine("r) Return to main");
                        Console.WriteLine("x) eXit");
                        break;
                    case MenuLevel.ShipsToPlace:
                        Console.WriteLine("x) Finish and eXit");
                        break;
                    default:
                        throw new Exception("Unknown location");
                }
                
                Console.Write(">");
                //handling user choice
                userChoice = Console.ReadLine()?.ToLower().Trim() ?? "";
                UserChoice = userChoice;
                if (userChoice == EXIT_CODE)
                {
                    Console.WriteLine("Closing ...");
                    break;
                } 

                //return userChosenMenuItem and boolean (whether item associated with the given key was found or not) + pass value of found entry into userChosenItem
                if (MenuItems.TryGetValue(userChoice, out var userChosenMenuItem))
                {
                    userChosenMenuItem.OptionToExecute();
                    break;
                } else {
                    Console.WriteLine("I don't have this option ");
                }
            } 
        }
    }
}