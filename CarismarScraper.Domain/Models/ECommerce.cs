using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarismarScraper.Domain.Models
{
    public class ECommerce:Portal
    {
        public ECommerce()
        {
            PortalUrl = "https://ecommerce.org";
            Categories = new Dictionary<string, string>();
            Companies = new List<Company>();
        }
    }
}
