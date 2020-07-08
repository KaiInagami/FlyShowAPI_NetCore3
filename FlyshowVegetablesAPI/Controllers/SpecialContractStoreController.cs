using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FlyshowVegetablesAPI.Controllers
{
    [ApiController]
    public class SpecialContractStoreController : ControllerBase
    {
        private ISpecialContractStoreService _specialContractStoreService;
        public SpecialContractStoreController(ISpecialContractStoreService specialContractStoreService)
        {
            _specialContractStoreService = specialContractStoreService;
        }

        /// <summary>
        /// Get SpecialContractStore Datas
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/SpecialContractStore/GetDatas"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public ResultObject GetDatas()
        {
           var datas = _specialContractStoreService.GetDatas();

            if (datas.Count <= 0)
            {
                return null;
            }

            ResultObject result = new ResultObject()
            {
                Total = datas.Count,
                Data = datas
            };

            return result;
        }

        [Route("api/v1/SpecialContractStore/CreateDatas"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public int CreateDatas(SpecialContractStore model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }

            return _specialContractStoreService.CreateDatas(model);
        }

        /// <summary>
        /// Delete SpecialContractStore Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Route("api/v1/SpecialContractStore/DeleteData"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool DeleteData(int ID)
        {
            if (ID <= 0)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }

            return _specialContractStoreService.DeleteData(ID);
        }
    }
}
