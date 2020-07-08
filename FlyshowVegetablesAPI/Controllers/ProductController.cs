using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Models.Request.Products;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Controllers
{
    [ApiController]
    public class ProductController
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 分頁取得商品列表 (要改大MODEL包小MODEL) 
        /// </summary>
        /// <param name="model"></param>
        [Route("api/v1/Product/GetProducts"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public ResultObject GetProducts(ProductsRequest model)
        {
            List<Product> products = _productService.GetProducts(model);
            GetCover(ref products);

            ResultObject result = new ResultObject() 
            {
                Total = products.Count,
                Data = products
            };

            return result;
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <param name="serialNo">商品序號</param>
        /// <returns></returns>
        [Route("api/v1/Product/GetProductBySerialNo"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public Product GetProductBySerialNo(string serialNo)
        {
            if (string.IsNullOrWhiteSpace(serialNo))
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

            return product;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="product">商品資料</param>
        /// <returns></returns>
        [Route("api/v1/Product/CreateProduct"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public int CreateProduct(Product product)
        {
            product.CreateDate = DateTime.Now;
            product.Image = JsonConvert.SerializeObject(new string[3]);

            if (string.IsNullOrEmpty(product.SerialNo))
            {
                product.SerialNo = string.Format("P-{0}-{1}", product.CreateDate.Value.ToString("yyyyMMdd"), Guid.NewGuid().ToString().ToUpper().Substring(0, 4));
            }

            return _productService.CreateProduct(product);
        }

        /// <summary>
        /// 編輯商品
        /// </summary>
        /// <param name="Product">商品資料</param>
        /// <returns></returns>
        [Route("api/v1/Product/UpdateProduct"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool UpdateProduct(Product product)
        {
            if (product.ID <= 0)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }    

            if (string.IsNullOrWhiteSpace(product.SerialNo))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }

            ProductsRequest productsRequest = new ProductsRequest();
            productsRequest.ProductCondition.SerialNo = product.SerialNo;

            if (_productService.GetProducts(productsRequest).FirstOrDefault() == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            return _productService.UpdateProduct(product);
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="SerialNo">商品序號</param>
        /// <returns></returns>
        [Route("api/v1/Product/DeleteProduct"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool DeleteProduct(string serialNo)
        {
            if (string.IsNullOrWhiteSpace(serialNo))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalRequest };
            }

            return _productService.DeleteProduct(serialNo);
        }


        #region Private Method
        /// <summary>
        /// 只保留第一張圖片當作封面
        /// </summary>
        /// <param name="products"></param>
        private void GetCover(ref List<Product> products)
        {
            foreach (Product product in products)
            {
                string images = product.Image;
                if (!string.IsNullOrEmpty(images) && images.Length > 2)
                {
                    images = images.Substring(1, images.Length - 2);
                    string[] imageArray = images.Split(',');

                    if (imageArray[0].IndexOf("null") == -1 && !string.IsNullOrEmpty(imageArray[0]))
                    {
                        product.Image = "[" + imageArray[0] + "]";
                    }
                    else if (imageArray[1].IndexOf("null") == -1 && !string.IsNullOrEmpty(imageArray[1]))
                    {
                        product.Image = "[" + imageArray[1] + "]";
                    }
                    else if (imageArray[2].IndexOf("null") == -1 && !string.IsNullOrEmpty(imageArray[2]))
                    {
                        product.Image = "[" + imageArray[2] + "]";
                    }
                    else
                    {
                        product.Image = "[null]";
                    }
                }
            }
        }
        #endregion
    }
}
