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
            string[] textFiles = Directory
                 .GetFiles(Path.Combine(
                     Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", string.Empty)), "*.txt")
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
                if (trainingCategory.Contains("Coalition"))
                {
                    coalitionTotal = coalitionTotal + 1;
                }
                else if (trainingCategory.Contains("Labour"))
                {
                    labourTotal = labourTotal + 1;
                }
                else if (trainingCategory.Contains("Conservative"))
                {
                    conservativeTotal = conservativeTotal + 1;
                }

            }
            double coalitionPriorProb = coalitionTotal / tDocCount;
            double labourPriorProb = labourTotal / tDocCount;
            double conservativePriorProb = conservativeTotal / tDocCount;

            DocumentProcessor.ProcessTextFiles(trainingCategories);
           
        }


        
    }
}//table = word|frequency|total|probability