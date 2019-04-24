using System;
using System.Collections.Generic;
using CSP.Consts;
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
                @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\DaneBadawcze\test_sky_6_0.txt");

            Console.WriteLine("Starting solving puzzle.");
            ISkyscrapper futoshiki = new SkyscrapperCSP();
            var result = futoshiki.SolveGame(data, Algorithm.Forwardchecking);
            Console.WriteLine("Successfuly solved puzzle.");

            try
            {
               fileHelper.WriteToFile(result.ToHtml(), "test_sky_6_0_fw_heuristic.html", @"C:\Users\domin\Desktop\Studia\Semestr VI\Sztuczna Inteligencja\Lab2\Wyniki\");
            }
            catch (FileAlreadyExistsException ex)
            {
               Console.WriteLine(ex.Message);
            }           
            Console.ReadKey();
        }
    }
}
