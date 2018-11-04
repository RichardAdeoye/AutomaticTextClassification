using System;
using System.Collections.Generic;
using System.IO;

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

                // Add count to table

                string tableName = null;

                if (trainingCategory.Contains("Labour"))
                {
                    //join the text in identical categories and total their count here somehow!

                    tableName = $@"{"Labour"}Table.csv";
                }
                else if (trainingCategory.Contains("Conservative"))
                {
                    tableName = $@"{"Conservative"}Table.csv";
                }
                else if (trainingCategory.Contains("Coalition"))
                {
                    tableName = $@"{"Coalition"}Table.csv";
                }

                string csvPath =
                    Path.Combine(
                        Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""),
                        tableName ?? throw new InvalidOperationException());

                File.AppendAllText(csvPath, csv);
            }
        }
    }
}