using System.Collections.Generic;
using System.Linq;

namespace consoletest
{
    public class Leaf : Component
    {
        public Leaf(string parsingCode, string text) : base(parsingCode, text)
        {
        }

        public override bool Operation() 
        {
            var split = ParsingCode.Split("&").Skip(1).ToList();
            var results = new List<bool>();
            var wordList = Text.Replace("  "," ").Split(" ");
            for (var i = 0; i < split.Count(); i++)
            {
                var wordsToBeContained = new List<string>();

                if (split[i].Contains('[') && split[i].Contains(']'))
                {
                    var openBracketIndex = split[i].IndexOf('[');
                    var closingBracketIndex = split[i].IndexOf(']');
                    if (openBracketIndex > closingBracketIndex)
                        throw new System.Exception("[ must be followed by ]");
                     
                    var singularWord = split[i].Substring(0,openBracketIndex);
                    wordsToBeContained.Add(singularWord);
                    
                    var variationsParsingCode = split[i].Substring(openBracketIndex+1, closingBracketIndex-openBracketIndex-1);
                    var variations = new List<string>();
                    if (variationsParsingCode.IndexOf("/") >= 0)
                        variations = variationsParsingCode.Split("/").ToList();
                    else
                        variations.Add(variationsParsingCode);

                    foreach (var variation in variations)
                        wordsToBeContained.Add($"{singularWord}{variation}");
                }
                else
                {
                    wordsToBeContained.Add(split[i]);
                }

                var anyContained = wordsToBeContained.Any(x => wordList.Contains(x));

                results.Add(anyContained);
            }
            return !results.Any(x => x==false);
        }

        public override bool IsComposite() => false;
    }
}