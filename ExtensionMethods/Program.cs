using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public class Program
    {
        internal delegate string Andrei(string input);

        internal static void Main(string[] args)
        {
            Andrei delegateOne = MethodExample;
            Console.WriteLine(delegateOne("Andrei"));

            var list = new List<int> { 1, 2, 3, 4 };
            foreach (var element in list.GetRange(0, 3).Select(n => n * n))
            {
                Console.WriteLine(element);
            }

            Console.Read();
        }

        private static string MethodExample(string name)
        {
            return name + "1";
        }
    }
}