using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                if (!file.Contains("test") && !file.Contains("stop"))
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

            DocumentProcessor.ProcessTextFiles(trainingCategories);
           
        }
    }
}//table = word|frequency|total|probability