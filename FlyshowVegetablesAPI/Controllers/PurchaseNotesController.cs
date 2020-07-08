using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace FlyshowVegetablesAPI.Controllers
{
    [ApiController]
    public class PurchaseNotesController
    {
        private static IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PurchaseNotesController(IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/PurchaseNotes/GetNPurchaseNotes"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public string GetPurchaseNotes()
        {
            try
            {
                string physicalPath = $"{_hostingEnvironment.ContentRootPath}{_config.GetValue<string>("PurchaseNotesPath")}";
                string fileName = "notes.txt";
                return FileManager.Read(fileName, physicalPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseNotes"></param>
        /// <returns></returns>
        [Route("api/v1/PurchaseNotes/SavePurchaseNotes"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool UpdatePurchaseNotes(string purchaseNotes)
        {
            try
            {
                string fileName = "notes.txt";
                string physicalPath = $"{_hostingEnvironment.ContentRootPath}{_config.GetValue<string>("PurchaseNotesPath")}";
                FileManager.Writer(purchaseNotes, fileName, physicalPath);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
