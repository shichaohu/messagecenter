using HS.Message.Share.CommonObject;
using HS.Message.Share.Http;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HS.Message.Service.@base
{
    /// <summary>
    /// 公告注入对象包
    /// </summary>
    public interface IInjectedObjects : IDependency
    {
        /// <summary>
        /// 请求上下文
        /// </summary>
        public IHttpContextAccessor Accessor { get; set; }
        /// <summary>
        /// 请求上下文信息（用户信息）
        /// </summary>
        public HttpContextInfo HttpContextInfo { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ServiceInjectedObjects : IInjectedObjects
    {
        /// <summary>
        /// 请求上下文
        /// </summary>
        public IHttpContextAccessor Accessor { get; set; }
        /// <summary>
        /// 请求上下文信息（用户信息）
        /// </summary>
        public HttpContextInfo HttpContextInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="accessor"></param>
        /// <param name="httpContextInfo"></param>
        public ServiceInjectedObjects(IHttpContextAccessor accessor, HttpContextInfo httpContextInfo)
        {
            Accessor = accessor;
            HttpContextInfo = httpContextInfo;
        }
    }
}
