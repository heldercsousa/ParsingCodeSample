using System;

namespace consoletest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            var parsingCode = "(&maçã&mamão)|(&banana&maracuj[ina/ás])";
            // var parsingCode = "&maçã&cocket | &maracujá[s]";
            // var parsingCode = "&java[s]&maçã&banana | &maçã";
            var client = new Client();
            var text = "Eu gosto de maçã e banana e maracujáss ou maracujina";
            client.BuildTree(parsingCode, text);
            var result = client.Tree.Operation();
            Console.WriteLine(result);
        }
    }
}
