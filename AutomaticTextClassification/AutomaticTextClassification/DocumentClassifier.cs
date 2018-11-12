using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutomaticTextClassification.DocumentProcessor;
using static AutomaticTextClassification.Program;
using static AutomaticTextClassification.ProcessingTools;
namespace AutomaticTextClassification
{
    class DocumentClassifier
    {
        private static Dictionary<string, double> CoalCondDictionary = new Dictionary<string, double>();
        private static Dictionary<string, double> ConservCondDictionary = new Dictionary<string, double>();
        private static Dictionary<string, double> LabCondDictionary = new Dictionary<string, double>();

        public static Dictionary<string, double> FileOptionDictionary;
        
        public static void ReadCSV()
        {
            Console.WriteLine("Enter the file you would like to undergo training:");

            //read from csv and train new network or use existing network
            string fileToTrain = Console.ReadLine();
            var trainingPath = Path.Combine(CurrentDirectory, fileToTrain);
            var x = File.ReadAllLines(trainingPath);
            foreach(var i in x)
            {
                Dictionary<string, int> tempfrequency = new Dictionary<string, int>();
                Dictionary<string, double> tempconditionalP = new Dictionary<string, double>();

                int frequency = 0;
                var splitData = i.Split(',');

                Int32.TryParse(splitData[1], out frequency);
                tempfrequency.Add(splitData[0], frequency );

                tempconditionalP.Add(splitData[0], Convert.ToDouble(splitData[2]));
            }
            //read text file or bayesian network
            //add the data to a temp dictionary
            //split on commas and store values in dictionary
            //b
        }
        public static void ClassifyDocument()
        {

            ReadCSV();///testing
            CreateConditionalProbDictionary(CoalDictionary, CoalitionConProb, CoalCondDictionary);
            CreateConditionalProbDictionary(ConservDictionary, ConservativeConProb, ConservCondDictionary);
            CreateConditionalProbDictionary(LabDictionary, LabourConProb, LabCondDictionary);
            //"test1.txt"
            Console.WriteLine("Enter the file you would like to be classified:");
            var fileOption = Console.ReadLine();
            string optionFilePath = Path.Combine(CurrentDirectory, fileOption);

            ProcessFileToList(optionFilePath, out fileOption, out var optionFileList);
            FileOptionDictionary = new Dictionary<string, double>();

            foreach (var item in optionFileList.Distinct())
            {
                if (item != "")
                {
                    double wordFrequency = optionFileList.Count(x => x == item);
                    FileOptionDictionary.Add(item, wordFrequency);
                }
            }


            double coalitionLogProb = 0f;
            double conservativeLogProb = 0f;
            double labourLogProb = 0f;
            foreach (var word in FileOptionDictionary.Keys)
            {
                if (CoalCondDictionary.ContainsKey(word))
                {
                    coalitionLogProb += Math.Log(Math.Pow(CoalCondDictionary[word], FileOptionDictionary[word]));
                }

                if (ConservCondDictionary.ContainsKey(word))
                {
                    conservativeLogProb += Math.Log(Math.Pow(ConservCondDictionary[word], FileOptionDictionary[word]));
                }

                if (LabCondDictionary.ContainsKey(word))
                {
                    labourLogProb += Math.Log(Math.Pow(LabCondDictionary[word], FileOptionDictionary[word]));
                }
            }

            coalitionLogProb += Math.Log(CoalitionPriorProb);
            conservativeLogProb += Math.Log(ConservativePriorProb);
            labourLogProb += Math.Log(LabourPriorProb);

            Console.WriteLine("Coalition Probability: " + coalitionLogProb);
            Console.WriteLine("Conservative Probability: " + conservativeLogProb);
            Console.WriteLine("Labour Probability: " + labourLogProb);

            Console.ReadLine();

            //state probability %
            //state which has higher value and what its classified as
        }

        public static void CreateConditionalProbDictionary(Dictionary<string, int> inputDictionary,
            List<double> conditionalProbability,
            Dictionary<string, double> outputDictionary)
        {
            int i = 0;
            foreach (var item in inputDictionary)
            {
                outputDictionary.Add(item.Key, conditionalProbability[i]);
                i++;
            }
        }

    }
}


