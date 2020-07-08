using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Models.Request.Advertise;
using FlyshowVegetablesAPI.Utilities;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Controllers
{
    [ApiController]
    public class AdvertiseController : ControllerBase
    {
        private static IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IAdvertiseService _advertiseService;

        public AdvertiseController(IConfiguration config, IWebHostEnvironment hostingEnvironment, IAdvertiseService advertiseService)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _advertiseService = advertiseService;
        }

        /// <summary>
        /// sssss
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/GetAdvertises"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public ResultObject GetAdvertises(AdvertiseCondition condition)
        {
            List<Advertise> advertise = _advertiseService.SearchAdvertises(condition);

            ResultObject result = new ResultObject()
            {
                Total = advertise.Count,
                Data = advertise
            };

            return result;
        }

        /// <summary>
        /// CreateAdvertise
        /// </summary>
        /// <param name="advertise"></param>
        [Route("api/v1/CreateAdvertise"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public int CreateAdvertise(Advertise advertise)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }

            string physicalPath = $"{_hostingEnvironment.ContentRootPath}{_config.GetValue<string>("ImagesUploadPath")}";
            advertise.ResourceUrl = CommonUtilities.GetAdvertiseImageUrlFromBase64(advertise.ResourceUrl, physicalPath, _config.GetValue<string>("ImagesUploadPath"));

            return _advertiseService.CreateAdvertise(advertise);
        }

        [Route("api/v1/DeleteAdvertise"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool DeleteAdvertise(int ID)
        {
            if (ID <= 0)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }

            string physicalPath = $"{_hostingEnvironment.ContentRootPath}{_config.GetValue<string>("ImagesUploadPath")}";

            AdvertiseCondition condition = new AdvertiseCondition();
            condition.ID = ID;

            Advertise advertise = _advertiseService.SearchAdvertises(condition).FirstOrDefault();

            if (advertise == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.ObjectNotFound };
            }

            CommonUtilities.DeleteImageFromUrl(advertise.ResourceUrl, physicalPath, _config.GetValue<string>("ImagesUploadPath"));

            return _advertiseService.DeleteAdvertise(ID);
        }
    }
}
