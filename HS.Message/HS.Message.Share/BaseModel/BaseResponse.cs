using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    public class BaseResponse
    {
        /// <summary>
        /// 返回状态的编码
        /// </summary>
        public ResponseCode Ret { get; set; }
        /// <summary>
        /// 详细数据
        /// </summary>
        public string Msg { get; set; }
    }
    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        /// <summary>
        /// 默认成功返回消息实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public BaseResponse<T> SuccessResponse<T>(T data, string message = "成功")
        {
            BaseResponse<T> responseDto = new BaseResponse<T>();
            responseDto.Ret = ResponseCode.Success;
            responseDto.Msg = message;
            responseDto.Data = data;
            return responseDto;
        }

        /// <summary>
        /// 默认失败返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public BaseResponse<T> FailResponse(string message, ResponseCode code = ResponseCode.ParameterError)
        {
            BaseResponse<T> responseDto = new BaseResponse<T>();
            responseDto.Ret = code;
            responseDto.Msg = message;
            return responseDto;
        }

    }

    public class TaskResponse<T>
    {
        public string AsyncState { get; set; }
        public bool CancellationPending { get; set; }
        //public string CreationOptions { get; set; }
        public string Exception { get; set; }
        public int Id { get; set; }
        public BaseResponse<T> Result { get; set; }
        //public string Status { get; set; }
    }
    public enum ResponseCode
    {
        /// <summary>
        /// 内部错误
        /// </summary>
        InternalError = 500,
        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 501,
        /// <summary>
        /// 其他错误
        /// </summary>
        OtherError = 502,
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError = 503,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 10000
    }

}
