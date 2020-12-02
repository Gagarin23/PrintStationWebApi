using System;
using System.Collections.Generic;
using System.Linq;
using DataBaseApi.Models;
using ServerCashe.Services;

namespace TestProj
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadLine();
        }

        static void Test(IEnumerable<string> str)
        {
            var enumerable = str.ToList();
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
