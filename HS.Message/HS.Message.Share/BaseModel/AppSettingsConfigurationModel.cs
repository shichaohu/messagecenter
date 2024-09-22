using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    /// <summary>
    /// appsetting.json配置
    /// </summary>
    public class AppSettingsConfigurationModel
    {
        public Jwt Jwt { get; set; }
        public Crm Crm { get; set; }
        public Azureblob AzureBlob { get; set; }
        public Logging Logging { get; set; }
        public Swaggerdocoptions SwaggerDocOptions { get; set; }
        public Externalapiurl ExternalApiUrl { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class Jwt
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpiryHours { get; set; }
    }

    public class Crm
    {
        public string resourceUrl { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string tenantId { get; set; }
        public string tokenUrl { get; set; }
    }

    public class Azureblob
    {
        public string connString { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }

    public class Swaggerdocoptions
    {
        public object BaseUrl { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Enabled { get; set; }
    }

    public class Externalapiurl
    {
        public string ECM { get; set; }
        public string SRDM { get; set; }
        public string PLM { get; set; }
    }

}
