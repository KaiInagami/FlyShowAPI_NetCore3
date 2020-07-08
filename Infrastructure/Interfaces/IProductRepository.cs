using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();

        bool UpdatePictures(Product product);

        int CreateProduct(Product product);

        bool UpdateProduct(Product product);

        bool DeleteProduct(string serialNo);
    }
}
