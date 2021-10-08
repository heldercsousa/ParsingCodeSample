using System.Collections.Generic;
using System.Linq;

namespace consoletest
{
    public class Composite : Component
    {
        public Component Tree { get; set; }

        private bool Not { get; set; } = false;

        public Composite(string parsingCode, string text) : base(parsingCode, text)
        {
            // resolve brackets
            var stack = new Stack<IndexedChar>();
            var parsingCodeReplaced = ParsingCode;

            foreach (var item in ParsingCodeIndexed)
            {
                if (item.Character == '(' || item.Character == ')')
                {
                    if (item.Character == '(')
                        stack.Push(item);

                    if (item.Character == ')')
                    {
                        if (!stack.Any())
                            throw new System.Exception($") unexpected. {ParsingCode}");

                        var openingBracket = stack.Pop();
                        if (item.Index - openingBracket.Index < 3)
                            throw new System.Exception($"() found with no expression within. {ParsingCode}");
                        
                        var subParsingCode = ParsingCode.Substring(openingBracket.Index+1, item.Index-openingBracket.Index-1);
                        
                        if (subParsingCode.Contains("(") || subParsingCode.Contains(")"))
                        {
                            var newBranch = new Composite(subParsingCode, text);
                            if (openingBracket.Index-1 >= 0 && ParsingCode[openingBracket.Index-1] == '!')
                                newBranch.Not = true;
                            Add(newBranch);
                        }
                        else
                        {
                            var newLeaf = new Leaf(subParsingCode, text);
                            Add(newLeaf);
                        }

                        parsingCodeReplaced = parsingCodeReplaced.Replace("("+subParsingCode+")", "COMPLEX");
                        stack.Clear();
                    }
                }
            }

            ParsingCode = parsingCodeReplaced;
            
            string addLeaf(Queue<IndexedChar> leafParsingCodeQueue)
            {
                var leafParsingCode = string.Join("",leafParsingCodeQueue.Select(x => x.Character.ToString()));
                leafParsingCodeQueue.Clear();
                
                var newLeaf = new Leaf(leafParsingCode, text);
                Add(newLeaf);

                return leafParsingCode;
            }

            // resolve leafs 
            var leafParsingCodeQueue = new Queue<IndexedChar>();
            foreach (var item in ParsingCodeIndexed)
            {
                if (item.Character == '&' && leafParsingCodeQueue.Count == 0)
                {
                    leafParsingCodeQueue.Enqueue(item);
                }
                else if (leafParsingCodeQueue.Count > 0)
                {
                    if (item.Character == '|')
                    {
                        var leafParsingCode = addLeaf(leafParsingCodeQueue);
                        ParsingCode = ParsingCode.Replace(leafParsingCode, "LEAF");
                    }
                    else
                    {
                        leafParsingCodeQueue.Enqueue(item);
                    }
                }
            }

            if (leafParsingCodeQueue.Any())            
            {
                var leafParsingCode = addLeaf(leafParsingCodeQueue);
                ParsingCode = ParsingCode.Replace(leafParsingCode, "LEAF");
            }
        }
        
        protected List<Component> _children = new List<Component>();
        
        public override void Add(Component component) => _children.Add(component);

        public override void Remove(Component component) => _children.Remove(component);

        public override bool Operation()
        {
            int i = 0;
            var results = new List<bool>();

            foreach (Component component in _children)
            {
                var result = component.Operation();
                if (Not)
                    result = !result;
                results.Add(result);
                i++;
            }
            
            return results.Any() && results.Any(x => x == true);
        }
    }
}