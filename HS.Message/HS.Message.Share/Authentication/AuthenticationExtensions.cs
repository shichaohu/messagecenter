using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HS.Message.Share.Authentication;

namespace HS.Message.Share.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationExtensions
    {

        /// <summary>
        /// Register the Authentication Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //完整的jwt Bearer鉴权方式
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = configuration["Jwt:Bearer:Issuer"],
            //        ValidateAudience = true,
            //        ValidAudience = configuration["Jwt:Bearer:Audience"],
            //        ValidateLifetime = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
            //    };
            //});
            //自定义鉴权方式
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthenticationHandler.CustomerSchemeName;
                options.DefaultChallengeScheme = CustomAuthenticationHandler.CustomerSchemeName;
                options.AddScheme<CustomAuthenticationHandler>(CustomAuthenticationHandler.CustomerSchemeName, CustomAuthenticationHandler.CustomerSchemeName);
            });
        }
    }
}
