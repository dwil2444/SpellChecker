using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SpellChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            int initialCapacity = 82765;
            int maxEditDistanceDictionary = 2; //maximum edit distance per dictionary precalculation
            var symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../SpellChecker/cuisinesdictionary.txt";
            if (!symSpell.LoadDictionary(path, 0, 1))
            {
                Console.Error.WriteLine("\rFile not found: " + Path.GetFullPath(path));
                Console.ReadKey();
                return;
            }
            //Console.WriteLine(symSpell.CreateDictionary(path));
            /*string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../SpellChecker/frequency_dictionary_en_82_765.txt";
            if (!symSpell.LoadDictionary(path, 0, 1))
            {
                Console.Error.WriteLine("\rFile not found: " + Path.GetFullPath(path));
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Found");
                Console.ReadLine()
            }*/
            string input;
            while (!string.IsNullOrEmpty(input = (Console.ReadLine() ?? "").Trim()))
            {
                Correct(input, symSpell);
            }
            Console.ReadLine();
        }
        public static void Correct(string input, SymSpell symSpell)
        {
            List<SymSpell.SuggestItem> suggestions = null;  // return the list of suggestions, await the list of suggestions

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //check if input term or similar terms within edit-distance are in dictionary, return results sorted by ascending edit distance, then by descending word frequency     
            const SymSpell.Verbosity verbosity = SymSpell.Verbosity.Closest;
            suggestions = symSpell.Lookup(input, verbosity);

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed.TotalMilliseconds.ToString("0.000") + " ms");

            //display term and frequency
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString() + " " + suggestion.count.ToString("N0"));
            }
            if (verbosity != SymSpell.Verbosity.Top) Console.WriteLine(suggestions.Count.ToString() + " suggestions");
        }
    }
}
