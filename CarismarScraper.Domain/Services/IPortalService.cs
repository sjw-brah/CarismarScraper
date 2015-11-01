using CarismarScraper.Domain.Models;
using System.Collections.Generic;

namespace CarismarScraper.Domain.Interface
{
    public interface IPortalService
    {
        Dictionary<string,string> GetCategoryList();
        void PopulateListOfURLs();
        void GetBasicInfo();
        void WriteToFile(Portal portal);
        Portal GetCurrentPortal();
    }
}
