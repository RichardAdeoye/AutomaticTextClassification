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

        static void Main(string[] args)
        {
            string[] textFiles = Directory
                 .GetFiles(Path.Combine(
                   CurrentDirectory), "*.txt")
                 .Select(Path.GetFileName)
                 .ToArray();

            List<string> trainingCategories = new List<string>();

            foreach (var file in textFiles)
            {
                if (!file.Contains("ClassifyDocument") && !file.Contains("stop"))
                {
                    trainingCategories.Add(file);
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

            ProcessFiles(trainingCategories, Coalition, CoalitionWordList, CoalDictionary, CoalitionConProb);
            ProcessFiles(trainingCategories, Conservative, ConservativeWordList, ConservDictionary, ConservativeConProb);
            ProcessFiles(trainingCategories, Labour, LabourWordList, LabDictionary, LabourConProb);

            MainMenu.RunMenu();
        }
    }
}