using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SpecialContractStoreRepository : ISpecialContractStoreRepository
    {
        private readonly AccessDBContext _dbContext;
        public SpecialContractStoreRepository(AccessDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All Datas
        /// </summary>
        /// <returns></returns>
        public IQueryable<SpecialContractStore> GetAll()
        {
            return _dbContext.SpecialContractStore.AsNoTracking();
        }

        /// <summary>
        /// Create Datas 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateDatas(SpecialContractStore model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SpecialContractStore.Add(model);

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
        /// Delete SpecialContractStore
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteByID(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    SpecialContractStore deleteEntity = _dbContext.SpecialContractStore.Where(model => model.ID.Equals(ID)).FirstOrDefault();
                    if (deleteEntity != null)
                    {
                        _dbContext.SpecialContractStore.Remove(deleteEntity);
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
