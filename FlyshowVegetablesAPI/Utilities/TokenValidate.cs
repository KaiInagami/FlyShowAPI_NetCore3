using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Services;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Jose;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Text;

namespace FlyshowVegetablesAPI.Utilities
{
    public class TokenValidate
    {
        //private static IConfiguration _config;
        //private  TokenRepository _tokenRepository;
        //private IUserService _userService;

        //public TokenValidate(IConfiguration config, TokenRepository tokenRepository)
        //{
        //    _config = config;
        //    _tokenRepository = tokenRepository;
        //}

        #region JWT
        ///// <summary>
        ///// check token is validate.
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns>if fail then return empty</returns>
        //public string CheckTokenIsRenew(string token)
        //{
        //    string ReturnValue = string.Empty;

        //    try
        //    {
        //        var jwtObject = JWT.Decode<JwtAuthObject>(
        //            token,
        //            Encoding.UTF8.GetBytes(_config.GetValue<string>("tokenManagement:secret")),
        //            JwsAlgorithm.HS256);

        //        DateTime expireTime;

        //        if (DateTime.TryParseExact(jwtObject.ExpireTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out expireTime))
        //        {
        //            if (DateTime.Compare(expireTime, DateTime.Now) < 0)
        //            {
        //                //Token無效
        //                throw new Exception("Token已逾期。");
        //            }
        //            else
        //            {
        //                if (!token.StartsWith("Bearer"))
        //                {
        //                    token = "Bearer " + token;
        //                }

        //                //token renew
        //                TimeSpan span = expireTime.Subtract(DateTime.Now);
        //                //判斷Token有效時間在30分以內，再延長30分鐘
        //                if (span.TotalMinutes <= 30)
        //                {
        //                    //delete old token 
        //                    _tokenRepository.RemoveToken(token);

        //                    //create new token
        //                    ReturnValue = CreateNewToken(jwtObject.Account);
        //                }
        //                else
        //                {
        //                    //未過期
        //                    ReturnValue = token;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentNullException("Token參數(ExpireTime)不正確");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //setErrorResponse(actionContext, ex.Message);
        //        throw new UnauthorizedAccessException("Token驗證錯誤:" + ex.Message);
        //    }

        //    return ReturnValue;
        //}

        ///// <summary>
        ///// Build New Token
        ///// </summary>
        ///// <param name="account"></param>
        ///// <returns></returns>
        //private static string CreateNewToken(string account)
        //{
        //    var CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
        //    var ExpireTime = DateTime.Now.AddMinutes(_config.GetValue<double>("tokenManagement:refreshExpiration")).ToString("yyyy-MM-dd HH:mm:ss").ToString();

        //    //將資料加入Token後加密
        //    var payload = new JwtAuthObject()
        //    {
        //        Account = account,
        //        CurrentTime = CurrentTime,
        //        //預設有效時間60分鐘
        //        ExpireTime = ExpireTime
        //    };

        //    string token = $"Bearer {JWT.Encode(payload, Encoding.UTF8.GetBytes(_config.GetValue<string>("tokenManagement:secret")), JwsAlgorithm.HS256)}";

        //    LogTokenInfo(token, account, CurrentTime, ExpireTime, CurrentTime, "1");

        //    return token;
        //}

        ////public string CheckTokenIsRenew(string token, out string userid, out string departmentCD, out string accountType, bool isrenew = false)
        ////{
        ////    string ReturnValue = "";

        ////    //思考中...

        ////    //// read key
        ////    //var secret = System.Configuration.ConfigurationManager.AppSettings["JwtSecretKey"];

        ////    //try
        ////    //{
        ////    //    var jwtObject = Jose.JWT.Decode<JwtAuthObject>(
        ////    //        //actionContext.Request.Headers.Authorization.Parameter,
        ////    //        token,
        ////    //        Encoding.UTF8.GetBytes(secret),
        ////    //        JwsAlgorithm.HS256);

        ////    //    userid = jwtObject.UserID;
        ////    //    departmentCD = jwtObject.DepartmentCD;
        ////    //    accountType = jwtObject.AccountType;

        ////    //    //string CurrentTime = jwtObject.CurrentTime;
        ////    //    //string ExpireTime = jwtObject.ExpireTime;
        ////    //    DateTime _ExpireTime;
        ////    //    if (DateTime.TryParseExact(jwtObject.ExpireTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _ExpireTime))
        ////    //    {
        ////    //        if (DateTime.Compare(_ExpireTime, DateTime.Now) < 0)
        ////    //        {
        ////    //            //Token無效
        ////    //            throw new Exception("Token已逾期。");
        ////    //        }
        ////    //        else
        ////    //        {
        ////    //            if (!token.StartsWith("Bearer"))
        ////    //                token = "Bearer " + token;

        ////    //            //檢查DB中的Token是否有效
        ////    //            if (!IsTokenAbandonInDB(token))
        ////    //            {
        ////    //                throw new ArgumentNullException("Token已被註銷。");
        ////    //            }

        ////    //            if (isrenew)   //是否需要自動更新
        ////    //            {
        ////    //                TimeSpan span = _ExpireTime.Subtract(DateTime.Now);
        ////    //                //判斷Token有效時間在30分以內，再延長30分鐘
        ////    //                if (span.TotalMinutes <= 30)
        ////    //                {
        ////    //                    //舊的Token取消
        ////    //                    ITokenRepository tokenRp = new TokenRepository();
        ////    //                    DetailRequestViewModel detailRequest = new DetailRequestViewModel();
        ////    //                    detailRequest.TokenJWT = token;
        ////    //                    //舊的Token設定為失效
        ////    //                    var renewToken = tokenRp.TokenAbandon(detailRequest);

        ////    //                    if (renewToken)
        ////    //                    {
        ////    //                        // 產生新Token
        ////    //                        ReturnValue = RefreshJWTToken(jwtObject.UserID, jwtObject.DepartmentCD, jwtObject.AccountType, _ExpireTime);
        ////    //                    }
        ////    //                }
        ////    //            }
        ////    //        }
        ////    //    }
        ////    //    else
        ////    //    {
        ////    //        throw new ArgumentNullException("Token參數(ExpireTime)不正確");
        ////    //    }
        ////    //}
        ////    //catch (Exception ex)
        ////    //{
        ////    //    //setErrorResponse(actionContext, ex.Message);
        ////    //    throw new UnauthorizedAccessException("Token驗證錯誤:" + ex.Message);
        ////    //}

        ////    return ReturnValue;
        ////}



        /////// <summary>
        /////// Login時產生token並放在http hrader中(有效時間預設60分)
        /////// </summary>
        /////// <param name="loginData"></param>
        ////public string GetTokenLogin(LoginData loginData)
        ////{
        ////    string token = "";
        ////    Int32 defaultExpireMinute = 60; //Token有效時間60分鐘(預設)

        ////    // TODO : 依照DB帳號/密碼比對後，取出UserID寫入Token中

        ////    // read jwt key
        ////    var secret = System.Configuration.ConfigurationManager.AppSettings["JwtSecretKey"];

        ////    var CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
        ////    var ExpireTime = DateTime.Now.AddMinutes(defaultExpireMinute).ToString("yyyy-MM-dd HH:mm:ss").ToString();
        ////    //將資料加入Token後加密
        ////    var payload = new JwtAuthObject()
        ////    {
        ////        UserID = loginData.UserId,
        ////        DepartmentCD = loginData.DepartmentCD,
        ////        AccountType = loginData.AccountType,
        ////        CurrentTime = CurrentTime,
        ////        //預設有效時間60分鐘
        ////        ExpireTime = ExpireTime
        ////    };

        ////    token = $"Bearer {Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256)}";

        ////    //放在header中
        ////    HttpContext.Current.Response.AddHeader("Authorization", token);

        ////    //var result = new ResponseModel.ApiResultModel()
        ////    //{
        ////    //    //Result = true,
        ////    //    Token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256),
        ////    //    Status = HttpStatusCode.OK,
        ////    //};

        ////    //放在Body的Data中
        ////    //return new
        ////    //{
        ////    //    Token = "Bearer " + Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256)
        ////    //};
        ////    LogTokenInfo(token, loginData.UserId, CurrentTime, ExpireTime, CurrentTime, loginData.UserId, "1");

        ////    return token;
        ////}


        /////// <summary>
        /////// 將 JWT Token 延長30分(有效時間低於30分)
        /////// </summary>
        /////// <param name="actionContext"></param>
        /////// <returns></returns>
        ////private string RefreshJWTToken(string userid, string departmentCD, string accountType, DateTime expiretime)
        ////{
        ////    // read key
        ////    var secret = System.Configuration.ConfigurationManager.AppSettings["JwtSecretKey"];

        ////    var CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
        ////    //延長30分鐘
        ////    var ExpireTime = expiretime.AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss").ToString();
        ////    //將資料加入Token後加密
        ////    var payload = new JwtAuthObject()
        ////    {
        ////        UserID = userid,
        ////        DepartmentCD = departmentCD,
        ////        AccountType = accountType,
        ////        CurrentTime = CurrentTime,
        ////        ExpireTime = ExpireTime
        ////    };

        ////    //var result = new ResponseModel.ApiResultModel()
        ////    //{
        ////    //    //Result = true,
        ////    //    Token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256),
        ////    //    Status = HttpStatusCode.OK,
        ////    //};

        ////    var Token = $"Bearer {Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256)}";

        ////    LogTokenInfo(Token, userid, CurrentTime, ExpireTime, CurrentTime, userid, "1");

        ////    return Token;
        ////}

        /////// <summary>
        /////// 判斷BD中的token是否已被取消
        /////// </summary>
        /////// <param name="token"></param>
        /////// <returns>true:有效, false:無效</returns>
        ////public bool IsTokenAbandonInDB(string token)
        ////{
        ////    var result = false;
        ////    PPPWEBAPI.Models.ViewModels.LoginSetting.DetailRequestViewModel detailRequest = new DetailRequestViewModel();
        ////    detailRequest.TokenJWT = token;
        ////    detailRequest.TokenStatus = "1";  //有效
        ////    ILoginSettingService loginSettingService = new LoginSettingService();
        ////    result = loginSettingService.QueryTokenByUserID(detailRequest);

        ////    return result;
        ////}

        /////// <summary>
        /////// 紀錄Token相關資訊(透過NLog寫入DB)
        /////// </summary>
        /////// <param name="tokenid"></param>
        /////// <param name="userid"></param>
        /////// <param name="effectivestartdate"></param>
        /////// <param name="effectiveenddate"></param>
        /////// <param name="createdate"></param>
        /////// <param name="createuserid"></param>
        /////// <param name="tokenstatus">true/false</param>
        /////// <returns></returns>
        //public static void LogTokenInfo(string tokenid, string userid, string effectivestartdate, string effectiveenddate, string createdate, string tokenstatus)
        //{
        //    Logger log = LogManager.GetLogger("TokenLog");
        //    LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, "", "message");
        //    theEvent.Properties["tokenid"] = tokenid;
        //    theEvent.Properties["userid"] = userid;
        //    theEvent.Properties["effectivestartdate"] = effectivestartdate;
        //    theEvent.Properties["effectiveenddate"] = effectiveenddate;
        //    theEvent.Properties["createdate"] = createdate;
        //    theEvent.Properties["tokenstatus"] = tokenstatus;   //預設是true
        //    log.Info(theEvent);
        //}

        #endregion


        /// <summary>
        /// VerifyToken
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool VerifyToken(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    return false;
                }

                DateTime exTime = Convert.ToDateTime(date);

                if (exTime >= DateTime.Now)
                {
                    //can get user to check
                    return true;
                    //return _userService.IsAccountExists(account);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
