using System;
using CarismarScraper.Domain.Services;
using CarismarScraper.Domain.Models;

namespace CarismarScraper.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            ECommerceService service = new ECommerceService(new ECommerce());
            Console.WriteLine("Retrieving categories...");
            service.GetCategoryList();
            service.PopulateListOfURLs();
            Console.WriteLine("Getting basic company info...");
            service.GetBasicInfo();
            Console.WriteLine("Getting info from AllaBolag.se...");
            service.GetInfoFromAllaBolag();
            service.WriteToFile(service.GetCurrentPortal());
            Console.ReadLine();
        
        }
    }
}
