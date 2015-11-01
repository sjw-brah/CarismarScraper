using CarismarScraper.Domain.Interface;
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using CarismarScraper.Domain.Models;

namespace CarismarScraper.Domain.Services
{
    public class ExcelWriter:IWriter
    {
        public _Application oXL { get; set; }
        public _Workbook oWB { get; set; }
        public _Worksheet oSheet { get; set; }
        public Range oRng { get; set; }

        public void WriteToFile(Portal portal)
        {
            object misValue = Missing.Value;
            oXL = new Application();
            oXL.Visible=true;

            oWB = (_Workbook)(oXL.Workbooks.Add(""));
            oSheet = (_Worksheet)oWB.ActiveSheet;

            oSheet.Cells[1, 1] = "Kategori";
            oSheet.Cells[1, 2] = "Portal URL";
            oSheet.Cells[1, 3] = "Company Name";
            oSheet.Cells[1, 4] = "OrgNr";
            oSheet.Cells[1, 5] = "Bolagsform";
            oSheet.Cells[1, 6] = "Antal Anställda";
            oSheet.Cells[1, 7] = "Omsättning (Tkr)";
            oSheet.Cells[1, 8] = "EShop";

            IList<Company> companies = portal.Companies;

            for(int i = 0; i < companies.Count; i++)
            {
                oSheet.Cells[i+2, 1] = companies[i].Category;
                oSheet.Cells[i+2, 2] = companies[i].PortalUrl;
                oSheet.Cells[i+2, 3] = companies[i].CompanyName;
                oSheet.Cells[i+2, 4] = companies[i].OrgNr;
                oSheet.Cells[i+2, 5] = companies[i].CorporateStructure;
                oSheet.Cells[i+2, 6] = companies[i].NumberOfEmployees;
                oSheet.Cells[i+2, 7] = companies[i].Turnover;
                oSheet.Cells[i+2, 8] = companies[i].URL;
            }

            oXL.Visible = false;
            oXL.UserControl = false;
            oWB.SaveAs(String.Format("CarismarScraper - {0}.xlsx",DateTime.Now.ToLongDateString()),
                XlFileFormat.xlWorkbookDefault, Type.Missing,Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, 
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,Type.Missing);
            oWB.Close();
        }
    }
}
