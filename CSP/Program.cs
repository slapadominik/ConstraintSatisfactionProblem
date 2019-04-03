using System;
using System.Collections.Generic;
using CSP.Entities;
using CSP.Helpers;

namespace CSP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IDataLoader<FutoshikiData> futoshikiLoader = new FutoshikiDataLoader(new FileHelper());
            futoshikiLoader.LoadFromFile(
                @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\CSP\data_training\futoshiki_4_0.txt");

            Console.ReadKey();
        }
    }
}
