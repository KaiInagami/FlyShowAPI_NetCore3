using System;
using System.Collections.Generic;
using System.Net;

namespace FlyshowVegetablesAPI.Models
{
    [Serializable]
    public class ApiResultModel
    {
        int _code;

        private static Dictionary<CodeEnum, string> Messages = new Dictionary<CodeEnum, string>(){
             { CodeEnum.OK, "成功" },
             { CodeEnum.Fail, "失敗" },
             { CodeEnum.EmailNotFilled, "信箱未填" },
             { CodeEnum.IllegalPasswordLength, "密碼不足8碼或是超過16碼" },
             { CodeEnum.PasswordNotFilled, "密碼未填" },
             { CodeEnum.AccountOrPasswordError, "帳號或是密碼錯誤" },
             { CodeEnum.AccountExist, "帳號已存在" },
             { CodeEnum.AccountNotExist, "帳號不存在" },
             { CodeEnum.RegisterFail, "註冊失敗" },
             { CodeEnum.IllegalToken, "Token失效" },
             { CodeEnum.ExpiredToken, "Token過期" },
             { CodeEnum.NoPermission, "無權限" },
             { CodeEnum.ObjectNotFound, "查無物件" },
             { CodeEnum.IllegalRequest, "輸入資料錯誤" },
        };
        public int Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
                Message = Messages[(CodeEnum)value];
            }
        }

        public string Message { get; set; }

        public string Token { get; set; }

        public object Data { get; set; }

        public enum CodeEnum
        {
            OK,
            Fail,
            EmailNotFilled,
            IllegalPasswordLength,
            PasswordNotFilled,
            AccountOrPasswordError,
            AccountExist,
            AccountNotExist,
            RegisterFail,
            IllegalToken,
            ExpiredToken,
            NoPermission,
            ObjectNotFound,
            IllegalRequest,
        }

        #region tmp
        ///// <summary>
        ///// Response Http Status Code
        ///// </summary>
        //public int Status { get; set; }
        ///// <summary>
        ///// Response Http Status Description
        ///// </summary>
        //public string StatusDescription { get; set; }
        ///// <summary>
        ///// Response data is here
        ///// </summary>
        //public object Data { get; set; }
        ///// <summary>
        ///// Response Error message
        ///// </summary>
        //public string ErrorMessage { get; set; }
        #endregion

    }

    public class ResultObject
    {
        public int Total { get; set; }

        public object Data { get; set; }
    }
}
