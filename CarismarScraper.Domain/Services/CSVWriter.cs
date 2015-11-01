using CarismarScraper.Domain.Interface;
using System;
using System.Text;
using System.IO;
using CarismarScraper.Domain.Models;

namespace CarismarScraper.Domain.Services
{
    public class CSVWriter:IWriter
    {
        public void WriteToFile(Portal portal)
        {
            var sw = new StreamWriter("CarismarScraper.csv");
            var sb = new StringBuilder("Kategori,Portal Url,Företagsnamn, EShop");
            string companyInfo = string.Empty;
            for(int i = 0; i < portal.Companies.Count; i++)
            {
                string category = portal.Companies[i].Category;
                string portalUrl = portal.Companies[i].PortalUrl;
                string companyName = portal.Companies[i].CompanyName;
                string companyUrl = portal.Companies[i].URL;
                companyInfo = String.Format("{0},{1},{2},{3}",category,portalUrl,companyName,companyUrl);

                sb.AppendLine(companyInfo);
            }

            sw.Write(companyInfo);
        }
    }
}
