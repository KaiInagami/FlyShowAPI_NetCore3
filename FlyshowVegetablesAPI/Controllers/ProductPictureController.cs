using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Models.Products;
using FlyshowVegetablesAPI.Models.Request.Products;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace FlyshowVegetablesAPI.Controllers
{
    public class ProductPictureController : ControllerBase
    {
        private IProductService _productService;
        public ProductPictureController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get Picture
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [Route("api/v1/ProductPicture/GetPicture"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public string GetPicture(string serialNo, int index = -1)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(serialNo) || index < -1 || index > 2)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            ProductsRequest productsRequest = new ProductsRequest();
            productsRequest.ProductCondition.SerialNo = serialNo;
            Product product = _productService.GetProducts(productsRequest).FirstOrDefault();

            if (product == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            if (index == -1)
            {
                result = product.Image;
            }
            else
            {
                string images = product.Image;
                if (images.Length > 2)
                {
                    images = images.Substring(1, images.Length - 2);
                    string[] imageArray = images.Split(',');
                    if (index < imageArray.Length)
                    {
                        result = $"[{imageArray[index]}]";
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Update Pictures
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [Route("api/v1/ProductPicture/UpdatePictures"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool UpdatePictures(ProductPicturesCondition condition)
        {
            if (string.IsNullOrWhiteSpace(condition.SerialNo) ||
               (string.IsNullOrWhiteSpace(condition.Pic1) && string.IsNullOrWhiteSpace(condition.Pic2) && string.IsNullOrWhiteSpace(condition.Pic3)))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            ProductsRequest productsRequest = new ProductsRequest();
            productsRequest.ProductCondition.SerialNo = condition.SerialNo;
            Product product = _productService.GetProducts(productsRequest).FirstOrDefault();

            if (product == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            product.Image = product.Image.Substring(1, product.Image.Length - 2);
            string[] currentImageArray = product.Image.Split(',');
            string[] preEditImageArray = new string[3];
            currentImageArray.CopyTo(preEditImageArray, 0);

            preEditImageArray[0] = condition.Pic1;
            preEditImageArray[1] = condition.Pic2;
            preEditImageArray[2] = condition.Pic3;

            product.Image = JsonConvert.SerializeObject(preEditImageArray);
            return _productService.UpdatePictures(product);
        }
    }
}
