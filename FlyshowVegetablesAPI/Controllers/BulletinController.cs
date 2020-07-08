using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Utilities;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace FlyshowVegetablesAPI.Controllers
{
    [ApiController]
    public class BulletinController : ControllerBase
    {
        private static IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BulletinController(IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>8
        [Route("api/v1/Bulletin/GetBulletin"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public string GetBulletin()
        {
            try
            {
                string physicalPath = $"{_hostingEnvironment.ContentRootPath}{_config.GetValue<string>("BulletinPath")}";
                string fileName = "bulletin.txt";
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
        /// <returns></returns>
        [Route("api/v1/Bulletin/Post"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool Post(string email, string bulletin)
        {
            try
            {
                if (!PermissionManager.Check(email))
                {
                    throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.NoPermission };
                }

                string fileName = "bulletin.txt";
                string physicalPath = $"{_hostingEnvironment.ContentRootPath}{_config.GetValue<string>("BulletinPath")}";
                FileManager.Writer(bulletin, fileName, physicalPath);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
