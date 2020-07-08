﻿using FlyshowVegetablesAPI.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public List<User> GetUser()
        {
            return _userRepository.GetAll().ToList();
        }

        /// <summary>
        /// Check account is exists or not
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsAccountExists(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                return false;
            }

            if (_userRepository.GetAll().Where(x => x.Email.Equals(account)).Count() <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateUser(User model)
        {
            if (model.Birthday.HasValue)
            {
                model.Birthday = DateTime.Parse(model.Birthday.Value.ToString("yyyyMMdd HH:mm:ss"));
            }

            return _userRepository.CreateUser(model);
        }

        /// <summary>
        /// UpdateUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool DeleteUser(int userID)
        {
            return _userRepository.DeleteUser(userID);
        }
    }
}
