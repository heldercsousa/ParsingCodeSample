using System.Collections.Generic;
using System.Linq;

namespace consoletest
{
    public abstract class Component
    {
        // public enum OperatorType    
        // {
        //     None,
        //     Or,
        //     And
        // }

        public IEnumerable<char> SpecialChars = new [] { '(', ')' };

        public string ParsingCode { get; set; }
        
        public IEnumerable<IndexedChar> ParsingCodeIndexed => ParsingCode.Select((x,y) => new IndexedChar { Character = x, Index = y});

        public string Text { get; set; }

        // public OperatorType Operator { get; set; }

        public Component(string parsingCode, string text)
        {
            ParsingCode = parsingCode;
            Text = text;
        }

        public abstract bool Operation();
        public virtual void Add(Component component) => throw new System.NotImplementedException();
        public virtual void Remove(Component component) => throw new System.NotImplementedException();
        public virtual bool IsComposite() => true;

        public static bool IsComplex(string parsingCode) => parsingCode.Contains("(") || parsingCode.Contains(")") || parsingCode.Contains("|");

        public class IndexedChar
        {
            public int Index { get; set; }
            public char Character { get; set; }
        }

        public override string ToString()
        {
            return ParsingCode;
        }
    }
}