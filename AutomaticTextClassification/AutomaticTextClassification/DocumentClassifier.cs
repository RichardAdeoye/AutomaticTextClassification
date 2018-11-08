using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutomaticTextClassification.DocumentProcessor;
using static AutomaticTextClassification.Program;

namespace AutomaticTextClassification
{
    class DocumentClassifier
    {
        Dictionary<string, double> CoalCondDictionary = new Dictionary<string, double>();
        Dictionary<string, double> ConservCondDictionary = new Dictionary<string, double>();
        Dictionary<string, double> LabCondDictionary = new Dictionary<string, double>();
        
        public static void test()
        {
            var a = CoalDictionary;
            var b = ConservDictionary;
            var c = LabDictionary;
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);

        }
        
       //public static Dictionary<string, double> TestDictionary;
       // public static Dictionary<string, double> LabDictionary = new Dictionary<string, double>();

       // public static Dictionary<string, double> ConservDictionary = new Dictionary<string, double>();

       // public static Dictionary<string, double> CoalDictionary =new Dictionary<string, double>();

       // public static void Classify()
       // {
       //     //CoalDictionary = new Dictionary<string, double>();
       //     //ConservDictionary = new Dictionary<string, double>();
       //     //LabDictionary = new Dictionary<string, double>();

       //     string testFilePath = Path.Combine(CurrentDirectory, "test1.txt");//console readline text file
       //     string testFile;
       //     List<string> testList;
       //     ProcessFileToList(testFilePath, out testFile, out testList);
       //     TestDictionary = new Dictionary<string, double>();

       //     foreach (var item in testList.Distinct())
       //     {
       //         if (item != "")
       //         {
       //             double wordFrequency = testList.Count(x => x == item);
       //             TestDictionary.Add(item, wordFrequency);
       //         }
       //     }

       //     double coalLogProb = 0f;
       //     double conLogProb = 0f;
       //     double labLogProb = 0f;
       //     double q;
       //     double w;
       //     foreach (var word in TestDictionary.Keys)
       //     {
       //         var a = ConservDictionary;
       //         var b = CoalDictionary;
       //         if (CoalDictionary.ContainsKey(word))
       //         {
       //          q = coalLogProb += Math.Log(Math.Pow(CoalDictionary[word], TestDictionary[word]));
       //         }
       //         if (ConservDictionary.ContainsKey(word))
       //         {
       //          conLogProb += Math.Log(Math.Pow(ConservDictionary[word], TestDictionary[word]));
       //         }
       //         if (LabDictionary.ContainsKey(word))
       //         {
       //          labLogProb += Math.Log(Math.Pow(LabDictionary[word], TestDictionary[word]));
       //         }
       //     }
       //    // Console.WriteLine(q);
       //     Console.WriteLine("conserve: " +conLogProb);
       //     Console.WriteLine("labour: "+ labLogProb);
       //   //  Console.ReadLine();

        }
    }


