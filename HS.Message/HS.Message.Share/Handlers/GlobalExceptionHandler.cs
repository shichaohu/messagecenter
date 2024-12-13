using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using HS.Message.Share.BaseModel;

namespace HS.Message.Share.Handlers
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 发送异常时的处理逻辑
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var response = new BaseResponse<dynamic>
            {
                Ret = ResponseCode.InternalError,
            };
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                response.Msg = context.Exception.Message;
                context.Result = new InternalServerErrorObjectResult(response);//返回异常数据
            }
            else
            {
                response.Msg = context.Exception.Message;
                context.Result = new InternalServerErrorObjectResult(response);
            }

            _logger.LogError($@"Exception：{context.Exception?.Message}:{context.Exception?.StackTrace}，InnerException：{context.Exception?.InnerException?.Message}");
        }
        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object error) : base(error)
            {
                StatusCode = StatusCodes.Status200OK;
            }
        }
    }
}
