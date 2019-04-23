using System;
using System.Collections.Generic;
using CSP.Entities;
using CSP.Entities.Futoshiki;
using CSP.Entities.Skyscrapper;
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
            FileHelper fileHelper = new FileHelper();
            IDataLoader<SkyscrapperData> futoshikiLoader = new SkyscrapperDataLoader(fileHelper);
            var data = futoshikiLoader.LoadFromFile(
                @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\CSP\data_training\skyscrapper_4_0.txt");

            Console.WriteLine("Starting solving puzzle.");
            ISkyscrapper futoshiki = new SkyscrapperCSP();
            var result = futoshiki.SolveGame(data);
            Console.WriteLine("Successfuly solved puzzle.");

            try
            {
               fileHelper.WriteToFile(result.ToHtml(), "skyscrapper_4_0.html", @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\Wyniki\");
            }
            catch (FileAlreadyExistsException ex)
            {
               Console.WriteLine(ex.Message);
            }           
            Console.ReadKey();
        }
    }
}
