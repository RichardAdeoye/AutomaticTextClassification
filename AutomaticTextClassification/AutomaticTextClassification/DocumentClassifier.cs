using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AutomaticTextClassification.DocumentProcessor;
using static AutomaticTextClassification.Program;
using static AutomaticTextClassification.ProcessingTools;
using  static AutomaticTextClassification.MainMenu;
namespace AutomaticTextClassification
{
    class DocumentClassifier
    {
        private static Dictionary<string, double> CoalCondDictionary = new Dictionary<string, double>();
        private static Dictionary<string, double> ConservCondDictionary = new Dictionary<string, double>();
        private static Dictionary<string, double> LabCondDictionary = new Dictionary<string, double>();

        public static Dictionary<string, double> testFileDictionary;
       
        public static void ClassifyDocument()
        {
          
            
            var tDocCount = trainingDocuments.Count();// gets count of total training documents available
            
            double coalitionTotal = 0D;
            double labourTotal = 0D;
            double conservativeTotal = 0D;
            foreach (var trainingCategory in trainingDocuments)// loops through all available documents
            {
                if (trainingCategory.Contains(Coalition))
                {
                    coalitionTotal = coalitionTotal + 1; // if the file contains the category name 1 is added to a variable
                }
                else if (trainingCategory.Contains(Labour))
                {
                    labourTotal = labourTotal + 1; // if the file contains the category name 1 is added to a variable
                }
                else if (trainingCategory.Contains(Conservative))
                {
                    conservativeTotal = conservativeTotal + 1; // if the file contains the category name 1 is added to a variable
                }
            }

            CoalitionPriorProb = coalitionTotal / tDocCount; // calculates prior probilitity

            LabourPriorProb = labourTotal / tDocCount; // calculates prior probilitity

            ConservativePriorProb = conservativeTotal / tDocCount; // calculates prior probilitity

            Console.WriteLine("Enter the Document you would like to be classified:");
            
            var fileOption = Console.ReadLine(); // reads users input for the test file they want classified
            string optionFilePath = Path.Combine(CurrentDirectory, fileOption); // creates a file path for the specified path
            if (File.Exists(optionFilePath))// checks if file exists
            {
                ProcessFileToList(optionFilePath, out fileOption, out var optionFileList);// coverts text file from path to list of words


                testFileDictionary = new Dictionary<string, double>();

                foreach (var item in optionFileList.Distinct())// loops through list of words
                {
                    if (item != "")
                    {
                        double wordFrequency = optionFileList.Count(x => x == item);// calculates frequency of each word 
                        testFileDictionary.Add(item, wordFrequency); //stores word and its frequency in a dictionary for test file
                    }
                }
            }
            else
            {
                Console.WriteLine("File Not Found! Enter valid file. e.g. filename.txt. Press ENTER to try again.");
                Console.ReadLine();
                Console.Clear();
                ClassifyDocument(); // reloads classification menu when file is not found
                
            }
            
            //ProcessFiles function from DocumentProcessor is called to process the files recieved from each category
            ProcessFiles(trainingDocuments, Coalition, Program.CoalitionWordList, CoalDictionary, CoalitionConProb);
            ProcessFiles(trainingDocuments, Conservative, Program.ConservativeWordList, ConservDictionary, ConservativeConProb);
            ProcessFiles(trainingDocuments, Labour, Program.LabourWordList, LabDictionary, LabourConProb);

            //CreateConditionalProbDictionary called to create dictionary of words and conditional probability from each category
            CreateConditionalProbDictionary(CoalDictionary, CoalitionConProb, CoalCondDictionary);
            CreateConditionalProbDictionary(ConservDictionary, ConservativeConProb, ConservCondDictionary);
            CreateConditionalProbDictionary(LabDictionary, LabourConProb, LabCondDictionary);

            double coalitionLogProb = 0f;
            double conservativeLogProb = 0f;
            double labourLogProb = 0f;
            foreach (var word in testFileDictionary.Keys) // loops through testFileDictionary keys
            {
                if (CoalCondDictionary.ContainsKey(word))// checks the training category dictionary to see if word from testFileDictionary exists
                {
                    coalitionLogProb += Math.Log(Math.Pow(CoalCondDictionary[word], testFileDictionary[word])); // uses Bayes formula to calculate the classification probability
                }

                if (ConservCondDictionary.ContainsKey(word))// checks the training category dictionary to see if word from testFileDictionary exists
                {
                    conservativeLogProb += Math.Log(Math.Pow(ConservCondDictionary[word], testFileDictionary[word]));// uses Bayes formula to calculate the classification probability
                }

                if (LabCondDictionary.ContainsKey(word))// checks the training category dictionary to see if word from testFileDictionary exists
                {
                    labourLogProb += Math.Log(Math.Pow(LabCondDictionary[word], testFileDictionary[word]));// uses Bayes formula to calculate the classification probability
                }
            }

            coalitionLogProb += Math.Log(CoalitionPriorProb); // multiplies classification probability by prior probability 
            conservativeLogProb += Math.Log(ConservativePriorProb); // multiplies classification probability by prior probability 
            labourLogProb += Math.Log(LabourPriorProb); // multiplies classification probability by prior probability 

            Console.WriteLine();
            //calculates and displays prior probability  %
            Console.WriteLine("Coalition Prior Probability : " + CoalitionPriorProb* 100 +"%");
            Console.WriteLine("Conservative Prior Probability : " + ConservativePriorProb* 100 +"%");
            Console.WriteLine("Labour Prior Probability : " + LabourPriorProb* 100 +"%");

            Console.WriteLine();
            // displays calculated probabilites
            Console.WriteLine("Coalition Probability: " + coalitionLogProb);
            Console.WriteLine("Conservative Probability: " + conservativeLogProb);
            Console.WriteLine("Labour Probability: " + labourLogProb);

            //checks which probabilies are higher and displays the classified government with highest probability value
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

            Console.WriteLine("Press ENTER to classify another Document or train a new file... ");
            Console.ReadLine();

            Console.Clear();
            RunMenu();// runs main menu
        }

        public static void CreateConditionalProbDictionary(Dictionary<string, double> inputDictionary,
            List<double> conditionalProbability,
            Dictionary<string, double> outputDictionary)
        {
            int i = 0;
            foreach (var item in inputDictionary)// loops through the input dictionary 
            {
              
                if (outputDictionary.ContainsKey(item.Key))// checks category dictionary for duplicate dictionary pairs
                {
                    outputDictionary[item.Key] = outputDictionary[item.Key] + conditionalProbability[i];     //// Merges duplicated dictionary pairs and their values

                }
                else
                {
                    outputDictionary.Add(item.Key, conditionalProbability[i]);// adds word and conditional probabilities to a dictionary
                    i++;
                }
               
            }
        }

    }
}


