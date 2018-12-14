using System.Collections.Generic;
using System.IO;
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

        public static List<string> CoalitionWordList = new List<string>();
        public static List<string> ConservativeWordList = new List<string>();
        public static List<string> LabourWordList = new List<string>();

        public static List<string> trainingDocuments = new List<string>();

        static void Main(string[] args)
        {
            
            RefreshCsvFiles();// Removes any old csv on program start to avoid existing files clash


            MainMenu.RunMenu();//Loads Main Menu
        }
    }
}