using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AccessDBContext _dbContext;
        public ProductRepository(AccessDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All Product
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> GetAll()
        {
            return _dbContext.Product.AsNoTracking();
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool UpdateProduct(Product product)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Product saveEntity = _dbContext.Product.Where(model => model.ID.Equals(product.ID)).FirstOrDefault();

                    saveEntity.Name = (!string.IsNullOrEmpty(product.Name)) ? product.Name : saveEntity.Name;
                    saveEntity.Unit = (!string.IsNullOrEmpty(product.Unit)) ? product.Unit : saveEntity.Unit;
                    saveEntity.Remark = (!string.IsNullOrEmpty(product.Remark)) ? product.Remark : saveEntity.Remark;
                    saveEntity.Area = (!string.IsNullOrEmpty(product.Area)) ? product.Area : saveEntity.Area;
                    saveEntity.IsInStock = (product.IsInStock > -1) ? product.IsInStock : saveEntity.IsInStock;
                    saveEntity.Type = (product.Type > -1) ? product.Type : saveEntity.Type;
                    saveEntity.YouTubeUrl = (!string.IsNullOrEmpty(product.YouTubeUrl)) ? product.YouTubeUrl : saveEntity.YouTubeUrl;

                    if (_dbContext.SaveChanges() <= 0)
                    {
                        return false;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        /// <summary>
        /// Update Pictures
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool UpdatePictures(Product product)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Product saveEntity = _dbContext.Product.Where(model => model.SerialNo.Equals(product.SerialNo)).FirstOrDefault();
                    saveEntity.Image = product.Image;

                    if (_dbContext.SaveChanges() <= 0)
                    {
                        return false;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int CreateProduct(Product model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Product.Add(model);

                    if (_dbContext.SaveChanges() <= 0)
                    {
                        return 0;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

                return model.ID;
            }
        }

        public bool DeleteProduct(string serialNo)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Product deleteEntity = _dbContext.Product.Where(model => model.SerialNo.Equals(serialNo)).FirstOrDefault();
                    if (deleteEntity != null)
                    {
                        _dbContext.Product.Remove(deleteEntity);
                    }

                    if (_dbContext.SaveChanges() <= 0)
                    {
                        return false;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

                return true;
            }
        }
    }
}
