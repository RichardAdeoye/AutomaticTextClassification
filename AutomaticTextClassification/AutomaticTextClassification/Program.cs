using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace AutomaticTextClassification
{
    class Program
    {
        static void Main(string[] args)
        {
            var textFilePath = Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), "Conservative27thMay2015.txt");
            //Create user input for files and separate training and test file variables
            var textFile = File.ReadAllText(textFilePath);

            var stopWordsFilePath = Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), "stopwords.txt");

            var stopWordsFile = File.ReadAllText(stopWordsFilePath);
            stopWordsFile = stopWordsFile.Replace("\r", " ");
            stopWordsFile = stopWordsFile.Replace("\n", "");

            var stopWords = stopWordsFile.Split(' '); 
            

            var trainingTextWords = textFile.Split(' ', ',', '.').Distinct();
           
            var trainingTextList = trainingTextWords.Where(i => !stopWords.Contains(i)).ToList();
            foreach (var word in textFile.Split(' ', ',', '.'))
            {
                if (word != "")
                {
                    var trainingWordFrequency = textFile.Split(' ', ',', '.').Count(x => x == word);
                    Console.WriteLine(word +": "+ trainingWordFrequency);
                }

            }

            Console.ReadLine();

        }
    }
}
