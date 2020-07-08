using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class AdvertiseRepository : IAdvertiseRepository
    {
        private readonly AccessDBContext _dbContext;
        public AdvertiseRepository(AccessDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        public IQueryable<Advertise> GetAll()
        {
            return _dbContext.Advertise.AsNoTracking();
        }

        /// <summary>
        /// CreateAdvertise
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateAdvertise(Advertise model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Advertise.Add(model);

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

        /// <summary>
        /// DeleteAdvertise
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteAdvertise(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Advertise deleteEntity = _dbContext.Advertise.Where(model => model.ID.Equals(ID)).FirstOrDefault();
                    if (deleteEntity != null)
                    {
                        _dbContext.Advertise.Remove(deleteEntity);
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
