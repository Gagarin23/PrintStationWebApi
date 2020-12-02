using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            var col1 = new string[]{"a", "b"};
            var col2 = new List<string> {"c", "d"};
            Test(col2);

            Console.ReadLine();
        }

        static void Test(IEnumerable<string> str)
        {
            var enumerable = str as string[] ?? str.ToArray();
            if (!enumerable.Any())
            {
                foreach (var txt in enumerable)
                {
                    Console.WriteLine(txt);
                }
            }
        }
    }
}
