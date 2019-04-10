using System;
using System.Collections.Generic;
using CSP.Entities;
using CSP.Entities.Futoshiki;
using CSP.Exceptions;
using CSP.Helpers;
using CSP.Problems;
using CSP.Problems.Interfaces;

namespace CSP
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataLoader<FutoshikiData> futoshikiLoader = new FutoshikiDataLoader(new FileHelper());
            var data = futoshikiLoader.LoadFromFile(
                @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\CSP\data_training\futoshiki_4_0.txt");

            IFutoshiki futoshiki = new FutoshikiCSP();

            var result = futoshiki.SolveGame(data);

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
