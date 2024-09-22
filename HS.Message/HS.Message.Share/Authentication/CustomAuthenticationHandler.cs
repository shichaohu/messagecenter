using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace HS.Message.Share.Authentication
{
    /// <summary>
    /// 自定义认证处理器
    /// </summary>
    public class CustomAuthenticationHandler : JwtBearerHandler
    {
        /// <summary>
        /// 自定义认证
        /// </summary>
        public const string CustomerSchemeName = "Basic";
        private readonly ILogger<CustomAuthenticationHandler> _logger;
        private IConfiguration _configuration;

        public CustomAuthenticationHandler(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory loggerFactory,
            ILogger<CustomAuthenticationHandler> logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, loggerFactory, encoder, clock)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Context.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string valStr = values.ToString();
            if (valStr.StartsWith(CustomerSchemeName, StringComparison.OrdinalIgnoreCase))
            {
                return BasicAuthenticateAsync();
            }
            else
            {

                var target = ResolveTarget(Options.ForwardAuthenticate);
                if (target != null)
                {

                }
                //Bearer 认证
                return base.HandleAuthenticateAsync();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task InitializeHandlerAsync()
        {
            Options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Bearer:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Bearer:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Bearer:SecretKey"]))
            };

            return Task.CompletedTask;
        }
        /// <summary>
        /// Basic认证 鉴权方式
        /// </summary>
        /// <returns></returns>
        public Task<AuthenticateResult> BasicAuthenticateAsync()
        {
            Context.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string valStr = values.ToString();
            var result = AuthenticateResult.Fail($"authenticate failed with value: {valStr},message:incorrect input authorization string");
            if (!string.IsNullOrWhiteSpace(valStr))
            {
                try
                {
                    string[]? allowIps = _configuration["Jwt:Basic:AllowIp"]?.ToString().Split(',');
                    var remoteIpAddressList = new List<string>();
                    var remoteIpAddress = Context.Connection.RemoteIpAddress?.ToString();
                    if (!string.IsNullOrWhiteSpace(remoteIpAddress)) remoteIpAddressList.Add(remoteIpAddress);
                    string[]? arryXForwardedFor = Context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',');
                    if (arryXForwardedFor?.Length > 0)
                    {
                        remoteIpAddressList.AddRange(arryXForwardedFor);
                    }
                    remoteIpAddressList = remoteIpAddressList.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    bool isIpAllowBasic = allowIps != null && (allowIps.Contains("*")
                        || allowIps.Any(x =>
                            remoteIpAddressList.Any(y => x.Contains(y.Trim()))
                            || remoteIpAddressList.Any(y => y.Contains(x.Trim()))
                            )
                        );

                    if (isIpAllowBasic)
                    {
                        bool validVale = false;
                        string[] authVal = Encoding.UTF8.GetString(Convert.FromBase64String(valStr[(CustomerSchemeName.Length + 1)..])).Split(':');
                        if (authVal.Length == 2)
                        {
                            validVale = authVal[0].Trim().ToLower() == _configuration["Jwt:Basic:Account"]?.ToLower();
                            validVale = validVale && authVal[1].Trim().ToLower() == _configuration["Jwt:Basic:Password"]?.ToLower();
                        }

                        if (validVale)
                        {
                            var ticket = GetAuthTicket(authVal[0], "Administration");
                            result = AuthenticateResult.Success(ticket);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = AuthenticateResult.Fail($"authenticate failed with value: {valStr}, message:{ex.Message}"); ;
                }

            }

            return Task.FromResult(result);
        }


        #region 认证校验逻辑

        /// <summary>
        /// 生成认证票据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private AuthenticationTicket GetAuthTicket(string name, string role)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role),
            }, CustomerSchemeName);
            var principal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationTicket(principal, Scheme.Name);
        }

        #endregion
    }

}
