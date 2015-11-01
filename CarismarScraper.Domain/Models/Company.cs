
namespace CarismarScraper.Domain
{
    public class Company
    {
        private string _numberOfEmployees;
        public string Category { get; set; }
        public string SiteName { get; set; }
        public string CompanyName { get; set; }
        public string CorporateStructure { get; set; }
        public string URL { get; set; }
        public string PortalUrl { get; set; }
        public string OrgNr { get; set; }
        public string Turnover { get; set; }
        public string NumberOfEmployees
        {
            get { return _numberOfEmployees; }
            set
            {
                if(!value.StartsWith("\t&nbsp;"))
                    _numberOfEmployees = value;
            }
        }

    }
}
