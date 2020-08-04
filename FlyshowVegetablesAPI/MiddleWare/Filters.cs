using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.Models;
using FlyshowVegetablesAPI.Utilities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlyshowVegetablesAPI.MiddleWare
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private IConfiguration _config;
        private ILogger<AuthorizationFilter> _logger;
        private TokenRepository _tokenRepository;
        private IUserService _userService;

        public AuthorizationFilter(IConfiguration config, ILogger<AuthorizationFilter> logger, TokenRepository tokenRepository, IUserService userService)
        {
            _config = config;
            _logger = logger;
            _tokenRepository = tokenRepository;
            _userService = userService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //string acc = "test@test.test";
            //DateTime extime = DateTime.Now.AddHours(24);
            //string tok = TokenValidate.EncryptToken(acc, extime);
            //bool chk = TokenValidate.VerifyToken(tok);

            var ss = context.HttpContext.Request.Body;

            //using (StreamReader reader = new StreamReader(ss, System.Text.Encoding.UTF8))
            //{
            //    var content = reader.ReadToEndAsync();

            //    var obj = Newtonsoft.Json.Linq.JObject.Parse(content);



            //}

            ApiResultModel result = new ApiResultModel();

            //from swagger test
            string token = token = context.HttpContext.Request.Query["Authorization"]; 

            if (string.IsNullOrWhiteSpace(token))
            {
                //from client request
                token = context.HttpContext.Request.Headers.Where(x => x.Key.Equals("Authorization")).FirstOrDefault().Value; 
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                result.Code = (int)ApiResultModel.CodeEnum.IllegalToken;
                _logger.LogError(result.Message);
                context.Result = new JsonResult(result);
            }
            else
            {
                #region flyshow version validate
                string[] accountAndLoginTime = GetDecryptData(token);
                string account = accountAndLoginTime[0];

                //check token date
                if (!TokenValidate.VerifyToken(accountAndLoginTime[1]))
                {
                    //fail
                    result.Code = (int)ApiResultModel.CodeEnum.IllegalToken;
                    _logger.LogError(result.Message);
                    context.Result = new JsonResult(result);
                }

                // check account exists
                if (!_userService.IsAccountExists(accountAndLoginTime[0]))
                {
                    result.Code = (int)ApiResultModel.CodeEnum.AccountNotExist;
                    _logger.LogError(result.Message);
                    context.Result = new JsonResult(result);
                }

                #endregion

                #region tmp
                //                //Get header->Authorization 
                //var Token = context.HttpContext.Request.Headers.Authorization.Parameter;
                //                string UserId;
                //                string DepartmentCD;
                //                string AccountType;
                //                TokenValidate tokenval = new TokenValidate();
                //                if (string.IsNullOrWhiteSpace(tokenval.CheckTokenIsRenew(Token, out UserId, out DepartmentCD, out AccountType, false)))
                //                {
                //                    var basecontroller = (_BaseController)actionContext.ControllerContext.Controller;
                //                    //權限:依UserID+Action去DB中取得相對應權限(可否Call Controller的權限)
                //#if !NOAUTH
                //                    IAuthorityService auth = new AuthorityService();
                //                    basecontroller._authState.IsAuth = auth.CheckActorPermission(UserId, controllerName);
                //#else
                //                                    basecontroller._authState.IsAuth = true;
                //#endif
                //                    basecontroller._authState.UserID = UserId;
                //                    basecontroller._authState.DepartmentCD = DepartmentCD;
                //                    basecontroller._authState.AccountType = AccountType;
                //                }
                #endregion
            }
        }

        private string[] GetDecryptData(string token)
        {
            return CommonUtilities.Decrypt(token).Split('_');
        }
    }

    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> _logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.RouteData.Values["controller"] as string;
            string actionName = context.RouteData.Values["action"] as string;
            _logger.LogInformation($"Action Call Controller: {controllerName}, Action: {actionName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Token Auth
            //context.HttpContext.Request.Headers.Count();

            //act return
            ApiResultModel result = new ApiResultModel();
            ObjectResult getObject = null;

            if (context.Exception != null)
            {
                if (context.Exception != null && context.Exception is HttpResponseException exception)
                {
                    //getObject = new ObjectResult(exception.Value)
                    //{
                    //    StatusCode = exception.Status,
                    //};

                    result.Code = exception.Status;
                    context.ExceptionHandled = true;
                }
                else
                {
                    if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        getObject = (Microsoft.AspNetCore.Mvc.ObjectResult)context.Result;
                        if (getObject != null && getObject.Value != null)
                        {
                            //success
                            result.Code = (int)ApiResultModel.CodeEnum.OK;
                            result.Data = getObject.Value;
                        }
                        else
                        {
                            //not found
                            result.Code = (int)ApiResultModel.CodeEnum.ObjectNotFound;
                        }
                    }
                    else
                    {
                        //fail
                        result.Code = (int)ApiResultModel.CodeEnum.Fail;
                    }
                }

                context.Result = new JsonResult(result);
            }
            else
            {
                return;
            }
        }
    }

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //context.HttpContext.Response.WriteAsync($"{context.Exception.Message}");
            //act return
            var result = new ApiResultModel();

            result.Code = (int)ApiResultModel.CodeEnum.Fail;

            context.Result = new JsonResult(result);
        }

        public MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
        }
    }

    public class HttpResponseException : Exception
    {
        public int Status { get; set; }

        public object Value { get; set; }
    }

    public class HeaderTokenOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Query,
                    //Description = "JWT Token",
                    Description = "Auth Token",
                    Required = false, // set to false if this is optional
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                        //Default = new Microsoft.OpenApi.Any.OpenApiString("Bearer {access token}")
                    }
                });
            }

        }
    }



    //public class ApiActionFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    //{
    //    /// <summary>
    //    /// Custom response format.
    //    /// </summary>
    //    /// <param name="actionExecutedContext"></param>
    //    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    //    {
    //        if (actionExecutedContext.Exception != null)
    //        {
    //            return;
    //        }

    //        base.OnActionExecuted(actionExecutedContext);

    //        #region Custom Response
    //        var result = new ApiResultModel();

    //        //get response status 
    //        result.Status = actionExecutedContext.ActionContext.Response.StatusCode;
    //        result.StatusDescription = actionExecutedContext.ActionContext.Response.StatusCode.ToString();

    //        /*
    //         *  example :
    //         *  var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "password validate faild." };
    //         *  throw new HttpResponseException(msg);
    //         */
    //        if (actionExecutedContext.ActionContext.Response.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            result.Data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;

    //            if (result.Data == null)
    //            {
    //                result.ErrorMessage = "No data result.";
    //            }
    //        }
    //        else
    //        {
    //            result.Data = true;
    //            result.ErrorMessage = actionExecutedContext.ActionContext.Response.ReasonPhrase;
    //        }

    //        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result.Status, result);

    //        #endregion
    //    }
    //}

    //public class ApiErrorHandleAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    //{
    //    //#region Fields
    //    //private LogService _logService;
    //    //public LogService LogService
    //    //{
    //    //    get
    //    //    {
    //    //        if (_logService == null)
    //    //        {
    //    //            _logService = IoC.Resolve<LogService>();
    //    //        }

    //    //        return _logService;
    //    //    }
    //    //}
    //    //#endregion

    //    /// <summary>
    //    ///  All Exception (except HttpResponseException) will be catched.
    //    /// </summary>
    //    /// <param name="actionExecutedContext"></param>
    //    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    //    {
    //        base.OnException(actionExecutedContext);

    //        string errorMessage = actionExecutedContext.Exception.Message;

    //        var result = new ApiResultModel()
    //        {
    //            Status = HttpStatusCode.BadRequest,
    //            StatusDescription = HttpStatusCode.BadRequest.ToString(),
    //            ErrorMessage = actionExecutedContext.Exception.Message
    //        };

    //        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result.Status, result);
    //    }
    //}
}
