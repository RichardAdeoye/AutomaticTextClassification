using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
            Console.WriteLine("Training Completed! Press ENTER to proceed to classification...");
            Console.ReadLine();
            Console.Clear();
            ClassifyDocument();
        }
        public static void ClassifyDocument()
        {

            CreateConditionalProbDictionary(CoalDictionary, CoalitionConProb, CoalCondDictionary);
            CreateConditionalProbDictionary(ConservDictionary, ConservativeConProb, ConservCondDictionary);
            CreateConditionalProbDictionary(LabDictionary, LabourConProb, LabCondDictionary);
          
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

            if (coalitionLogProb > conservativeLogProb && coalitionLogProb > labourLogProb)
            {
                Console.WriteLine("\n This Queen's Speech shows it is a Coalition Government.");
            }
            else if (conservativeLogProb > coalitionLogProb && conservativeLogProb > labourLogProb)
            {
                Console.WriteLine("\n This Queen's Speech shows it is a Conservative Government.");
            }
            else
            {
                Console.WriteLine("\n This Queen's Speech shows it is a Labour Government.");
            }
            Console.ReadLine();

            //STATE PROBABILITY %
            //STATE WHICH HAS HIGHER VALUE AND WHAT ITS CLASSIFIED AS
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


