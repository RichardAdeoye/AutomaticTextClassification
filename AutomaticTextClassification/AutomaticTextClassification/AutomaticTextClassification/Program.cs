using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace AutomaticTextClassification
{
    class Program
    {
        static void Main(string[] args)
        {
            //var trainingCatergories = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory()));

            var textFilePath = Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), "Conservative27thMay2015.txt");
            //Create user input for files and separate training and test file variables
            var textFile = File.ReadAllText(textFilePath);
            textFile = textFile.Replace("\n", "");
            textFile = textFile.Replace("\r", "");
            var trainingTextWords = textFile.Split(' ', ',', '.').Distinct().ToList();//Unique words in training set

            var textWords = textFile.Split(' ', ',', '.').ToList();

            List<string> trainingTextList = RemoveStopWords(textWords).ToList();

            List<int> frequency = new List<int>();
            List<string> words = new List<string>();

            foreach (var word in trainingTextList)// total number of unique words throughout the training documents (Frequency)
            {
                int trainingWordFrequency;
                    if (word != "")
                    {
                        trainingWordFrequency = trainingTextList.Count(x => x == word);
                        //Console.WriteLine(word + ": " + trainingWordFrequency);
                    words.Add(word);
                    frequency.Add(trainingWordFrequency);
                    
                    };
                        

            }
            var i = 0;
            foreach(var x in words)
            {
                if (words.Contains(x))
                {
                    Console.WriteLine(x + ":" + frequency[i]);
                    i++;
                }
                
            }

            var csv = new StringBuilder();
            csv.AppendLine(trainingTextList.ToString());
            string csvPath =
                Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""),
                    "tables.csv");

            File.AppendAllText(csvPath, csv.ToString());
            Console.ReadLine();

        }

        private static List<string> RemoveStopWords(List<string> trainingTextWords)
        {
            var stopWordsFilePath = Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), "stopwords.txt");

            var stopWordsFile = File.ReadAllText("K:\\AutomaticTextClassification\\AutomaticTextClassification\\stopwords.txt");
            stopWordsFile = stopWordsFile.Replace("\n", "");
           // stopWords = stopWordsFile.Split(' ');
            string[] x = stopWordsFile.Split();
           

            var trainingTextList = trainingTextWords.Where(i => !x.Contains(i)).ToList();//stop words removed from list
            return trainingTextList;
        }
    }
}
