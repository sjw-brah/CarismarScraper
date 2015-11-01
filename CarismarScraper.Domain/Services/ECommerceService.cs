using CarismarScraper.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Diagnostics;
using CarismarScraper.Domain.Models;
using Microsoft.Win32;

namespace CarismarScraper.Domain.Services
{
    public class ECommerceService : IPortalService, IWriter
    {
        private Portal _eCommerce;
        public ECommerceService(Portal eCom)
        {
            _eCommerce = eCom;
        }
        public Portal GetCurrentPortal()
        {
            return _eCommerce;
        }

        #region General Public Methods
        public Dictionary<string, string> GetCategoryList()
        {
            var web = new HtmlWeb().Load(_eCommerce.PortalUrl+"/sv/butiksguiden/");
            var elements = web.DocumentNode.SelectNodes("//a/img[@src='../../../5.0.0.0/218/files/arrowRight.png']/..");

            if(elements == null)
                return _eCommerce.Categories;

            foreach(HtmlNode element in elements)
            {
                string name = GetCategoryName(element);
                _eCommerce.Categories.Add(name, element.Attributes[0].Value);
            }
            return _eCommerce.Categories;
        }
        public void PopulateListOfURLs()
        {
            foreach(var category in _eCommerce.Categories)
            {
                var web = new HtmlWeb().Load(_eCommerce.PortalUrl+category.Value);
                var SWEelements = web.DocumentNode.SelectNodes("//a/div/div/img[@src='../../../5.0.0.0/530/files/Flags small/SWE.png']/../../..");
                var DANelements = web.DocumentNode.SelectNodes("//a/div/div/img[@src='../../../5.0.0.0/149/files/Flags small/DEN.png']/../../..");
                var NORelements = web.DocumentNode.SelectNodes("//a/div/div/img[@src='../../../5.0.0.0/530/files/Flags small/NOR.png']/../../..");
                var FINelements = web.DocumentNode.SelectNodes("//a/div/div/img[@src='../../../5.0.0.0/530/files/Flags small/FIN.png']/../../..");

                if(SWEelements!=null) AddUrlsToCompany(SWEelements, category.Key);
                if(DANelements!=null) AddUrlsToCompany(DANelements, category.Key);
                if(NORelements!=null) AddUrlsToCompany(NORelements, category.Key);
                if(FINelements!=null) AddUrlsToCompany(FINelements, category.Key);
            }
        }
        public void GetBasicInfo()
        {
            foreach(var company in _eCommerce.Companies)
            {
                var web = new HtmlWeb().Load(_eCommerce.PortalUrl+company.PortalUrl);
                HtmlNode companyNode = web.DocumentNode.SelectSingleNode("//td[@class='NORMAL O7eef984c']/div/span");
                HtmlNode companyUrl = web.DocumentNode.SelectSingleNode("//td[@class='NORMAL Oe3052756']/div/a");
                if(companyNode==null && companyUrl==null)
                {
                    HandleCaptcha(company);
                    web = new HtmlWeb().Load(_eCommerce.PortalUrl+company.PortalUrl);
                    companyNode = web.DocumentNode.SelectSingleNode("//td[@class='NORMAL O7eef984c']/div/span");
                    companyUrl = web.DocumentNode.SelectSingleNode("//td[@class='NORMAL Oe3052756']/div/a");
                }

                company.CompanyName = companyNode ==null?"":companyNode.InnerHtml;
                company.URL = companyUrl ==null?"":companyUrl.InnerHtml;
            }
        }
        public void WriteToFile(Portal portal)
        {
            IWriter writer = new ExcelWriter();
            writer.WriteToFile(portal);
        }
        #endregion
        #region Methods Related To AllaBolag.se
        //These should all be extracted to a seperate 'AllaBolag'service class
        public void GetInfoFromAllaBolag()
        {
            List<string> filterList =new List<string>{
                "DANSKA NATBUTIKER","NORSKA NATBUTIKER","FINSKA NATBUTIKER","NATBUTIKER I KONKURS"};

            foreach(var company in _eCommerce.Companies)
            {
                if(filterList.Any(c => company.Category.Contains(c)))
                    continue;

                string link = GetLinkToCorporateProfile(company.CompanyName);
                if(string.IsNullOrEmpty(link))
                    continue;
                ScrapeCompanyProfile(link, company);

            }
        }
        private string GetLinkToCorporateProfile(string companyName)
        {
            var result = "";
            var web = new HtmlWeb().Load("http://www.allabolag.se/?what="
                +FormatCompanyNameUrl(companyName));

            HtmlNode companyNode = web.DocumentNode.SelectSingleNode("//td[@id='hitlistName']/a");

            if(companyNode!=null)
            {
                string htmlName = CleanUpCompanyName(companyNode.Attributes[1].Value);
                if(htmlName== companyName.ToUpper())
                    result = companyNode.Attributes[0].Value;
            };
            return result;
        }
        private void ScrapeCompanyProfile(string profileLink, Company company)
        {
            try
            {
                var web = new HtmlWeb().Load(profileLink);
                HtmlNodeCollection infoTable = web.DocumentNode.SelectNodes("//tr[@class='bgLightPink']");
                foreach(HtmlNode tr in infoTable)
                {
                    for(int i = 0; i < tr.ChildNodes.Count; i++)
                    {
                        if(tr.ChildNodes[i].InnerText=="Antal anst�llda")
                        { company.NumberOfEmployees = tr.ChildNodes[i+2].InnerText; continue; }

                        if(tr.ChildNodes[i].InnerText=="Oms�ttning (TKR)")
                        { company.Turnover = tr.ChildNodes[i+2].InnerText; continue; }
                    }
                }

                company.OrgNr = web.DocumentNode.SelectSingleNode("//span[@id='printOrgnr']").InnerText ?? "";
                company.CorporateStructure= web.DocumentNode.SelectSingleNode("//span[@id='printType']").InnerText ?? "";

            }
            catch(NullReferenceException e)
            {

                Console.WriteLine("{0}: fanns inte på AllaBolag.se", e.Source);
            }

        }
        #endregion
        #region Private Methods
        private void AddUrlsToCompany(HtmlNodeCollection nodes, string category)
        {
            foreach(var node in nodes)
            {
                _eCommerce.Companies.Add(new Company
                {
                    PortalUrl = node.Attributes[0].Value,
                    Category = category
                });
            }
        }

        private string FormatCompanyNameUrl(string companyName)
        {
            companyName = companyName.ToUpper().
                Replace("Ö", "%F6").Replace("Ä", "%E4").
                Replace("Å", "%C5").Replace(" ", "+").
                Replace("&", "%26").Replace(":", "%3A").
                Replace(",", "%2C");

            return companyName;
        }
        private string CleanUpCompanyName(string name)
        {
            name = name.ToUpper().
                Replace("&ARING;", "Å").Replace("&AUML;", "Ä").
                Replace("&OUML;", "Ö").Replace("&AMP;", "&");
            return name;
        }
        private void HandleCaptcha(Company company)
        {

            var psi = new ProcessStartInfo("iexplore.exe");
            
            psi.Arguments = _eCommerce.PortalUrl+company.PortalUrl;
            Process.Start(psi).WaitForExit();
        }
        
        #endregion
        static string GetCategoryName(HtmlNode element)
        {
            string name = element.Attributes[0].Value.Substring(17);
            return name.Replace('-', ' ').Replace("/", "").ToUpperInvariant();
        }
    }
}
