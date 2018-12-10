using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AutomaticTextClassification.ProcessingTools;
using static AutomaticTextClassification.DocumentProcessor;
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

        public static List<string> CoalitionWordList = new List<string>();
        public static List<string> ConservativeWordList = new List<string>();
        public static List<string> LabourWordList = new List<string>();

        public static List<string> trainingCategories = new List<string>();

        static void Main(string[] args)
        {
            string[] textFiles = Directory
                 .GetFiles(Path.Combine(
                   CurrentDirectory), "*.txt")
                 .Select(Path.GetFileName)
                 .ToArray();         

            foreach (var file in textFiles)
            {
                if (!file.Contains("ClassifyDocument") && !file.Contains("stop") && !file.Contains("test"))
                {
                    trainingCategories.Add(file);
                }
            }

            var tDocCount = trainingCategories.Count();

            double coalitionTotal = 0D;
            double labourTotal = 0D;
            double conservativeTotal = 0D;
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


            MainMenu.RunMenu();
        }
    }
}