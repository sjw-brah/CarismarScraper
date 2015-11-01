using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarismarScraper.Domain.Interface;
using CarismarScraper.Domain.Models;
using CarismarScraper.Domain.Services;

namespace CarismarScraper.Test
{
    [TestClass]
    public class PortalTest
    {
        static ECommerceService sut = new ECommerceService(new ECommerce());
        [TestMethod]
        public void ListOfCategoriesContainsData()
        {
            var result = sut.GetCategoryList();
        }
        [TestMethod]
        public void ListOfUrlContainsData()
        {
            sut.PopulateListOfURLs();
        }
        [TestMethod]
        public void GetBasicDataReturnsASpan()
        {
            sut.GetBasicInfo();
        }
        
    }
}
