using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AutomaticTextClassification.Program;

namespace AutomaticTextClassification
{
    class FileWriter
    {
        public static void WriteToCsv(string trainingCategory, Dictionary<string, double> wordDictionary, List<double> conditionalProbabilities)
        {
            int i = 0;
            foreach (var data in wordDictionary.Distinct())// Loops through word dictionary data
            {
                string tableName = null;
                
                if (trainingCategory.Contains(Labour))// checks if the category the data is coming from is Labour
                {
                    tableName = $@"{Labour}Table.csv";//builds csv file name
                }
                else if (trainingCategory.Contains(Conservative))// checks if the category the data is coming from is Conservative
                {
                    tableName = $@"{Conservative}Table.csv";//builds csv file name
                }
                else if (trainingCategory.Contains(Coalition))// checks if the category the data is coming from is Coalition
                {
                    tableName = $@"{Coalition}Table.csv";//builds csv file name
                }
                
                string csvPath =
                    Path.Combine(
                       CurrentDirectory,
                        tableName ?? throw new InvalidOperationException());//builds directory path with the category file name

                var csv = string.Concat(data.Key + "," + data.Value + "," + conditionalProbabilities[i], Environment.NewLine); 
                //creates csv string with word, frequency and conditional probability
                i++;

                File.AppendAllText(csvPath, csv);// creates the Bayesian csv file of the given category's csv path and apendeds the csv strings into that csv file
            }
          
        }
    }
}