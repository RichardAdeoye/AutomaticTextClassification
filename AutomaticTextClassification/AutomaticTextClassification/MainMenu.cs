using System;
using System.Text;

namespace AutomaticTextClassification
{
    class MainMenu
    {
        public static void RunMenu()
        {
            string mainTitle = "_Automatic Text Classification_";
            StringBuilder titleLine = new StringBuilder();
            titleLine.Length = Console.WindowWidth;
            titleLine.Insert((titleLine.Length - mainTitle.Length) / 2, mainTitle, 1);// Centres Title
        
            Console.Title = mainTitle;
            Console.WriteLine(titleLine.ToString());

            Console.WriteLine(" Menu");
            Console.WriteLine("========================================================================================================================");

            string[] Options; // Array of user options
            Options = new string[2];
            Options[0] = "Classify Text File";
            Options[1] = "Undertake Training";

            Console.WriteLine("Please Select an option by pressing 1 or 2, then hit ENTER:");
            Console.WriteLine("1. " + Options[0].ToString()); // Prints Option
            Console.WriteLine("2. " + Options[1].ToString()); // Prints Option
            Console.WriteLine();


            string usersSelection;
            usersSelection = Console.ReadLine();

            // IMPROVE MENU, ADD PERCENTAGES AND MORE ERROR VALIDATIONS
            
                if (usersSelection == "1") // Reads user input and calls Classify document function
                {
                    Console.Clear();
                    DocumentClassifier.ClassifyDocument();
                }
                else
                if (usersSelection == "2") // Reads user input and calls Read Csv document function
            {
                    Console.Clear();
                    DocumentClassifier.ReadCSV();
                }
                else
                {
                    Console.WriteLine("Please Enter either 1 or 2...Press ENTER to reset");
                    Console.ReadLine();
                    Console.Clear();
                    RunMenu();
                }
            
        }
    }
}