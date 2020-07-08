using FlyshowVegetablesAPI.Interfaces;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyshowVegetablesAPI.Utilities
{
    public class PermissionManager
    {

        private static IUserService _userSservice;

        public PermissionManager(IUserService userSservice)
        {
            _userSservice = userSservice;
        }

        public enum Role
        {
            /// <summary>
            /// Admin
            /// </summary>
            Admin = 74970855,

            /// <summary>
            /// Nomal
            /// </summary>
            Nomal = 1,
        }

        /// <summary>
        /// 檢查權限
        /// </summary>
        /// <param name="target">受檢者</param>
        /// <returns></returns>
        public static bool Check(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            User user = _userSservice.GetUser().Where(user => user.Email.Equals(email)).FirstOrDefault();

            if (!user.Priority.Equals(Role.Admin))
            {
                return false;
            }

            return true;

            //foreach (Role role in permissions)
            //{
            //    if (user.Priority == (int)role)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
    }
}
