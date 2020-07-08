using FlyshowVegetablesAPI.Models.Request.Products;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace FlyshowVegetablesAPI.Interfaces
{
    public interface IProductService
    {
        List<Product> GetProducts(ProductsRequest model);

        bool UpdatePictures(Product product);

        int CreateProduct(Product product);

        bool UpdateProduct(Product product);

        bool DeleteProduct(string serialNo);
    }
}
