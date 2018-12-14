using System;
using System.IO;
using System.Linq;
using System.Text;
using static AutomaticTextClassification.Program;
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
            Options = new string[3];
            Options[0] = "Undertake Training & Classification";
            Options[1] = "Undertake Classification (From Prior Training Phase)";
            Options[2] = "Quit Program";

            Console.WriteLine("Please Select an option by pressing 1, 2 or 3, then hit ENTER:");
            Console.WriteLine("1. " + Options[0].ToString()); // Prints Option
            Console.WriteLine("2. " + Options[1].ToString()); // Prints Option
            Console.WriteLine("3. " + Options[2].ToString()); // Prints Option
            Console.WriteLine();


            string usersSelection;
            usersSelection = Console.ReadLine();

         
                if (usersSelection == "1") // Reads user input and calls Classify document function
                {
                    Console.Clear();
                    RunTrainingMenu();
                }
                else
                if (usersSelection == "2") // Reads user input and calls Read Csv document function
                {
                    Console.Clear();
                    if (trainingDocuments.Count == 0)
                    {
                        Console.WriteLine("Bayesian Network was not found! Return to Menu to undertake training. Press ENTER to proceed.");
                        Console.ReadLine();
                        Console.Clear();
                        RunMenu();
                    }
                    else
                    {
                    DocumentClassifier.ClassifyDocument();
                    }
                    
                }
                else
                if (usersSelection == "3") // Reads user input and calls Read Csv document function
                {
                Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Please Enter a valid input(1, 2 or 3)...Press ENTER to reset");
                    Console.ReadLine();
                    Console.Clear();
                    RunMenu();
                }
            
        }

        public static void RunTrainingMenu()
        {
            string[] Options; // Array of user options
            Options = new string[2];
            Options[0] = "Current Directory";
            Options[1] = "Type in Directory Path";

            Console.WriteLine("Select the directory of the documents you would like to train from (Enter Number): ");
            Console.WriteLine("1. " + Options[0].ToString());
            Console.WriteLine("2. " + Options[1].ToString());

          

            var usersSelection = Console.ReadLine();
            if (usersSelection == "1") // Reads user input and calls Classify document function
            {
                string[] directoryPath = Directory
                    .GetFiles(Path.Combine(
                        CurrentDirectory), "*.txt")
                    .Select(Path.GetFileName)
                    .ToArray();// gets all text files from current directory

                foreach (var file in directoryPath)
                {
                    if (!file.Contains("stop") && !file.Contains("test"))
                    {
                        trainingDocuments.Add(file);// adds text files to list if they do not contain stop or test so unwanted text files are not read
                    }

                }
                foreach (var trainingDocument in trainingDocuments.Distinct())
                {
                    Console.WriteLine(trainingDocument);// lists training documents found
                }

                Console.WriteLine("\n Training Documents Found. A Bayesian Network will be written from these files. \n Press ENTER to proceed to Classification!");
                Console.ReadLine();
                Console.Clear();
                DocumentClassifier.ClassifyDocument();// executes document classifiyer

            }
            else
            {
                Console.WriteLine("Enter Valid Input! (e.g. 1 or 2).");
                Console.ReadLine();
                Console.Clear();
                RunTrainingMenu();// reloads document training menu 
            }


        }
    }
}