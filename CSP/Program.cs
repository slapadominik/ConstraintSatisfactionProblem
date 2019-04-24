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
            IDataLoader<FutoshikiData> futoshikiLoader = new FutoshikiDataLoader(fileHelper);
            var data = futoshikiLoader.LoadFromFile(
                @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\DaneBadawcze\test_futo_9_0.txt");

            Console.WriteLine("Starting solving puzzle.");
            IFutoshiki futoshiki = new FutoshikiCSP();
            var result = futoshiki.SolveGame(data);
            Console.WriteLine("Successfuly solved puzzle.");

            try
            {
               fileHelper.WriteToFile(result.ToHtml(), "test_futo_9_0_fw.html", @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\Wyniki\");
            }
            catch (FileAlreadyExistsException ex)
            {
               Console.WriteLine(ex.Message);
            }           
            Console.ReadKey();
        }
    }
}
