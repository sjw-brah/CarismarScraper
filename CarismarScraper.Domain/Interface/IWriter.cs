using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarismarScraper.Domain.Models;

namespace CarismarScraper.Domain.Interface
{
    public interface IWriter
    {
        void WriteToFile(Portal portal);
    }
}
