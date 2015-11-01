using System.Collections.Generic;

namespace CarismarScraper.Domain.Models
{
    public abstract class Portal
    {
        public string PortalUrl;
        public Dictionary<string, string> Categories;
        public IList<Company> Companies;
    }
}
