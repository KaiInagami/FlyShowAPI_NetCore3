using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.Models.Request.Products;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetProducts(ProductsRequest request)
        {
            var products = _productRepository.GetAll();

            if (request.ProductCondition.ID > 0)
            {
                products = products.Where(model => model.ID.Equals(request.ProductCondition.ID));
            }

            if (!string.IsNullOrWhiteSpace(request.ProductCondition.SerialNo))
            {
                products = products.Where(model => model.SerialNo.Equals(request.ProductCondition.SerialNo));
            }

            if (!string.IsNullOrWhiteSpace(request.ProductCondition.Name))
            {
                products = products.Where(model => model.Name.Equals(request.ProductCondition.Name));
            }

            if (request.ProductCondition.Price > 0)
            {
                products = products.Where(model => model.Price.Equals(request.ProductCondition.Price));
            }

            if (!string.IsNullOrWhiteSpace(request.ProductCondition.Unit))
            {
                products = products.Where(model => model.Unit.Equals(request.ProductCondition.Unit));
            }

            if (request.ProductCondition.Inventory > 0)
            {
                products = products.Where(model => model.Inventory.Equals(request.ProductCondition.Inventory));
            }

            if (!string.IsNullOrWhiteSpace(request.ProductCondition.Remark))
            {
                products = products.Where(model => model.Remark.Equals(request.ProductCondition.Remark));
            }

            if (!string.IsNullOrWhiteSpace(request.ProductCondition.Area))
            {
                products = products.Where(model => model.Area.Equals(request.ProductCondition.Area));
            }

            if (request.ProductCondition.IsInStock > 0)
            {
                products = products.Where(model => model.IsInStock > 0);
            }

            if (request.ProductCondition.Type > 0)
            {
                products = products.Where(model => model.Type.Equals(request.ProductCondition.Type));
            }

            products = products.Skip(request.StartItem - 1).Take(request.Length);

            return products.ToList();
        }

        public bool UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }

        public bool UpdatePictures(Product product)
        {
            return _productRepository.UpdatePictures(product);
        }

        public int CreateProduct(Product product)
        {
            return _productRepository.CreateProduct(product);
        }

        public bool DeleteProduct(string serialNo)
        {
            return _productRepository.DeleteProduct(serialNo);
        }
    }
}
