using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Models.Request;
using FlyshowVegetablesAPI.Utilities;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userSservice;
        private TokenRepository _tokenRepository;

        public UserController(IUserService service, TokenRepository tokenRepository)
        {
            _userSservice = service;
            _tokenRepository = tokenRepository;
        }
        #region Public Method

        /// <summary>
        /// TEST
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/User/GetUser"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public List<User> GetUser()
        {


            //var test = _tokenRepository.Get();

            //exception..
            //try
            //{
            //    int e = 1;

            //    int qq = e / 0;
            //}
            //catch (System.Exception ex)
            //{

            //    throw ex;
            //}

            //var msg = new HttpResponseException() { Value = "test" };
            //throw new HttpResponseException() { Status = 2, Value = "test" };

            var users = _userSservice.GetUser().ToList();

            return users;

            //return null;
        }

        [Route("api/v1/User/Register"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public int Register(User model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.EmailNotFilled };
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.PasswordNotFilled };
            }
            if (model.Password.Length < 8 || model.Password.Length > 16)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalPasswordLength };
            }
            if (_userSservice.IsAccountExists(model.Email))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.AccountExist };
            }

            model.Priority = 1;

            int userID = _userSservice.CreateUser(model);

            if (userID == -1)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.RegisterFail };
            }

            return userID;
        }

        /// <summary>
        /// Unregister
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("api/v1/User/Unregister"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool Unregister(int userID)
        {
            if (!_userSservice.DeleteUser(userID))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            return true;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/v1/User/Login"), HttpPost]
        [TypeFilter(typeof(ActionFilter))]
        public ApiResultModel Login(LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Account) || string.IsNullOrEmpty(model.Password) || model.Password.Length < 8 || model.Password.Length > 16)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.AccountOrPasswordError };
            }

            User user = _userSservice.GetUser().Where(user => user.Email.Equals(model.Account)).FirstOrDefault();

            if (user == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.AccountNotExist };
            }

            if (!CommonUtilities.Decrypt(user.Password).Equals(model.Password))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.AccountOrPasswordError };
            }

            return new ApiResultModel() { Code = (int)ApiResultModel.CodeEnum.OK, Token = CommonUtilities.EncryptToken(model.Account, DateTime.Now) };


            #region tmp
            //var secret = _config.GetValue<string>("tokenManagement:secret");

            //var CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
            //var ExpireTime = DateTime.Now.AddMinutes(defaultExpireMinute).ToString("yyyy-MM-dd HH:mm:ss").ToString();

            ////將資料加入Token後加密
            //var payload = new JwtAuthObject()
            //{
            //    Account = request.Account,
            //    CurrentTime = CurrentTime,
            //    //預設有效時間60分鐘
            //    ExpireTime = ExpireTime
            //};

            //token = $"Bearer {Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256)}";

            //token = TokenValidate.

            //放在header中
            //Response.Headers.Add("Authorization", token);



            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Name,request.Account)
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var jwtToken = new JwtSecurityToken(
            //    _tokenManagement.Issuer,
            //    _tokenManagement.Audience,
            //    claims,
            //    expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
            //    signingCredentials: credentials);
            //var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            //return Ok(token);
            #endregion
        }

        /// <summary>
        /// ReadUserInfo
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("api/v1/User/ReadUserInfo"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public User ReadUserInfo(string email)
        {
            User result = _userSservice.GetUser().Where(user => user.Email.Equals(email)).FirstOrDefault();

            if (result == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalToken };
            }

            result.Password = string.Empty;

            return result;
        }

        /// <summary>
        /// UpdateUserInfo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/v1/User/UpdateUserInfo"), HttpPost]
        [TypeFilter(typeof(AuthorizationFilter))]
        [TypeFilter(typeof(ActionFilter))]
        public bool UpdateUserInfo(User model)
        {
            User modifyUser = _userSservice.GetUser().Where(user => user.Email.Equals(model.Email)).FirstOrDefault();

            if (modifyUser == null)
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.IllegalToken };
            }

            modifyUser.Name = model.Name;
            modifyUser.Phone = model.Phone;
            modifyUser.Address = model.Address;
            modifyUser.Gender = model.Gender;
            if (modifyUser.Birthday.HasValue)
            {
                modifyUser.Birthday = model.Birthday;
            }

            bool isEdited = _userSservice.UpdateUser(modifyUser);

            if (!_userSservice.UpdateUser(modifyUser))
            {
                throw new HttpResponseException() { Status = (int)ApiResultModel.CodeEnum.Fail };
            }

            return true;
        }

        #endregion
    }
}
