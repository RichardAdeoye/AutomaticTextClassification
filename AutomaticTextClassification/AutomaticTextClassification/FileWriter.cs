using System;
using System.Collections.Generic;
using System.IO;
using static AutomaticTextClassification.Program;

namespace AutomaticTextClassification
{
    class FileWriter
    {
        public static void WriteToCsv(string trainingCategory, Dictionary<string, int> wordDictionary, List<double> conditionalProbabilities)
        {
            int i = 0;
            foreach (var data in wordDictionary)
            {
                var csv = string.Concat(data.Key + "," + data.Value + "," + conditionalProbabilities[i], Environment.NewLine);
                i++;

                string tableName = null;

                if (trainingCategory.Contains(Labour))
                {
                    tableName = $@"{Labour}Table.csv";
                }
                else if (trainingCategory.Contains(Conservative))
                {
                    tableName = $@"{Conservative}Table.csv";
                }
                else if (trainingCategory.Contains(Coalition))
                {
                    tableName = $@"{Coalition}Table.csv";
                }

                string csvPath =
                    Path.Combine(
                       CurrentDirectory,
                        tableName ?? throw new InvalidOperationException());

                File.AppendAllText(csvPath, csv);
            }
          
        }
    }
}