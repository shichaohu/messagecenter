using HS.Message.Share.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HS.Message.Share.CommonObject
{
    /// <summary>
    /// 公告注入对象包
    /// </summary>
    public interface IRepositoryInjectedObjects : IDependency
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; set; }
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
    public class RepositoryInjectedObjects : IRepositoryInjectedObjects
    {

        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; set; }
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
        /// <param name="configuration"></param>
        /// <param name="accessor"></param>
        /// <param name="httpContextInfo"></param>
        public RepositoryInjectedObjects(IConfiguration configuration, IHttpContextAccessor accessor, HttpContextInfo httpContextInfo)
        {
            Configuration = configuration;
            Accessor = accessor;
            HttpContextInfo = httpContextInfo;
        }
    }
}
