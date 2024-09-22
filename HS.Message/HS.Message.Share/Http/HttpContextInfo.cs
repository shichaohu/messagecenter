using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using HS.Message.Share.Utils;

namespace HS.Message.Share.Http
{
    /// <summary>
    /// http请求信息
    /// </summary>
    public class HttpContextInfo
    {
        private readonly IHttpContextAccessor _accessor;
        protected const string _excleContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        protected const string _excleDirectory = "excleUpload";
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        private string _tokenKey;

        private Dictionary<string, dynamic> _token;
        public HttpContextInfo(IHttpContextAccessor accessor, IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            _accessor = accessor;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public string tokenKey
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_tokenKey))
                    {
                        _tokenKey = _configuration["TokenKey"];
                        if (string.IsNullOrEmpty(_tokenKey))
                        {
                            _tokenKey = "cQcqLSHk46ZsmTz5U8nRVEV4";
                        }
                    }
                }
                catch (Exception)
                {
                }

                return _tokenKey;
            }
        }

        public Dictionary<string, dynamic> Token
        {
            get
            {
                if (_token != null)
                {
                    return _token;
                }

                StringValues stringValues = _accessor.HttpContext.Request.Headers["token"];
                if (string.IsNullOrWhiteSpace(stringValues))
                {
                    return null;
                }

                try
                {
                    _token = JsonConvert.DeserializeObject<Dictionary<string, object>>(EncryptUtil.DecodeDES(stringValues, tokenKey));
                }
                catch (Exception)
                {
                    _token = null;
                }

                return _token;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetTokenValuByKey(string key)
        {
            if (Token == null || !Token.ContainsKey(key))
            {
                return null;
            }

            return Token[key];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetTokenNumValuByKey(string key)
        {
            try
            {
                if (Token == null || !Token.ContainsKey(key))
                {
                    return 0;
                }

                return (int)Token[key];
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// user id
        /// </summary>
        /// <returns></returns>
        public long GetUserId()
        {
            if (Token == null)
            {
                return 0L;
            }

            return GetTokenNumValuByKey("userid");
        }
        /// <summary>
        /// user name
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            if (Token == null)
            {
                return "系统自动";
            }

            return GetTokenValuByKey("username")?.ToString() + string.Empty;
        }
        /// <summary>
        /// request ip
        /// </summary>
        /// <returns></returns>
        public string GetRequestIp()
        {
            var forwarded = _accessor.HttpContext.Request.Headers["X-Forwarded-For"];
            string text = forwarded.Any() ? forwarded.FirstOrDefault() : string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                text = _accessor.HttpContext.Connection?.RemoteIpAddress?.ToString();
            }

            return text;
        }
        /// <summary>
        /// 机场三字码
        /// </summary>
        /// <returns></returns>
        public string GetBelongAirportThree()
        {
            if (Token == null)
            {
                return "";
            }

            return GetTokenValuByKey("belong_airport_three")?.ToString() ?? string.Empty;
        }
    }
}
