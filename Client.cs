namespace consoletest
{
    public class Client
    {
        public Component Tree { get; set; }

        public void BuildTree(string parsingCode, string text)
        {
            parsingCode = parsingCode.Trim().Replace(" ","");

            if (Component.IsComplex(parsingCode))
                Tree = new Composite(parsingCode, text);
            else
                Tree = new Leaf(parsingCode, text);
        }
    }
}