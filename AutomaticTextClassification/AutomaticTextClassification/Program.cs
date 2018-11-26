using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static AutomaticTextClassification.ProcessingTools;
namespace AutomaticTextClassification
{
    class Program
    {
        public const string Coalition = "Coalition";
        public const string Conservative = "Conservative";
        public const string Labour = "Labour";

        public static string CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory()
            .Replace("\\AutomaticTextClassification\\bin\\Debug", ""));

        public static double CoalitionPriorProb;
        public static double LabourPriorProb;
        public static double ConservativePriorProb;

        static void Main(string[] args)
        {
            string[] textFiles = Directory
                 .GetFiles(Path.Combine(
                   CurrentDirectory), "*.txt")
                 .Select(Path.GetFileName)
                 .ToArray();

            List<string> trainingCategories = new List<string>();// get prior probability of files

            foreach (var file in textFiles)
            {
                if (!file.Contains("ClassifyDocument") && !file.Contains("stop"))
                {
                    trainingCategories.Add(file);
                   // Console.WriteLine(file);
                }
            }

            var tDocCount = trainingCategories.Count();

            double coalitionTotal = 0f;
            double labourTotal = 0f;
            double conservativeTotal = 0f;
            foreach (var trainingCategory in trainingCategories)
            {
                if (trainingCategory.Contains(Coalition))
                {
                    coalitionTotal = coalitionTotal + 1;
                }
                else if (trainingCategory.Contains(Labour))
                {
                    labourTotal = labourTotal + 1;
                }
                else if (trainingCategory.Contains(Conservative))
                {
                    conservativeTotal = conservativeTotal + 1;
                }

            }
           
            CoalitionPriorProb = coalitionTotal / tDocCount;
           
            LabourPriorProb = labourTotal / tDocCount;
            
            ConservativePriorProb = conservativeTotal / tDocCount;
            
            RefreshCsvFiles();

            DocumentProcessor.ProcessCoalitionFiles(trainingCategories);
            DocumentProcessor.ProcessConservativeFiles(trainingCategories);
            DocumentProcessor.ProcessLabourFiles(trainingCategories);
            MainMenu();
        }

        public static void MainMenu()
        {
            string mainTitle = "_Automatic Text Classification_";
            StringBuilder titleLine = new StringBuilder();
            titleLine.Length = Console.WindowWidth;
            titleLine.Insert((titleLine.Length - mainTitle.Length) / 2, mainTitle, 1);
        
            Console.Title = mainTitle;
            Console.WriteLine(titleLine.ToString());

            Console.WriteLine(" Menu");
            Console.WriteLine("========================================================================================================================");

            string[] Options;
            Options = new string[2];
            Options[0] = "Classify Text File";
            Options[1] = "Undertake Training";

            Console.WriteLine("Please Select an option by pressing 1 or 2, then hit ENTER:");
            Console.WriteLine("1. " + Options[0].ToString());
            Console.WriteLine("2. " + Options[1].ToString());
            Console.WriteLine();


            string usersSelection;
            usersSelection = Console.ReadLine();


            {
                if (usersSelection == "1")
                {
                    Console.Clear();
                    DocumentClassifier.ClassifyDocument();
                }
                else
                if (usersSelection == "2")
                {
                    Console.Clear();
                    DocumentClassifier.ReadCSV();
                }
                else
                {
                    Console.WriteLine("Please Enter either 1 or 2...Press ENTER to reset");
                    Console.ReadLine();
                    Console.Clear();
                    MainMenu();
                }
            }
            
        }
    }
}