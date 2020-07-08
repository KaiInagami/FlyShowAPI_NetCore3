using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AccessDBContext _dbContext;
        public UserRepository(AccessDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetAll()
        {
            return _dbContext.User.AsNoTracking();
        }

        /// <summary>
        /// CreateUser
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateUser(User model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                     _dbContext.User.Add(model);

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
            }

            return model.ID;
        }

        /// <summary>
        /// UpdateUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(User user)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    User saveEntity = _dbContext.User.Where(model => model.Email.Equals(user.Email)).FirstOrDefault();

                    saveEntity.Name = !string.IsNullOrWhiteSpace(user.Name) ? user.Name : saveEntity.Name;
                    saveEntity.Gender = user.Gender.HasValue ? user.Gender : saveEntity.Gender;
                    saveEntity.Phone = !string.IsNullOrWhiteSpace(user.Phone) ? user.Phone : saveEntity.Phone;
                    saveEntity.Birthday = user.Birthday.HasValue ? user.Birthday : saveEntity.Birthday;
                    saveEntity.Address = !string.IsNullOrWhiteSpace(user.Address) ? user.Address : saveEntity.Address;
                    saveEntity.Priority = user.Priority.HasValue ? user.Priority : saveEntity.Priority;

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
        /// DeleteUser
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool DeleteUser(int userID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    User deleteEntity = _dbContext.User.Where(model => model.ID.Equals(userID)).FirstOrDefault();
                    if (deleteEntity != null)
                    {
                        _dbContext.User.Remove(deleteEntity);
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
