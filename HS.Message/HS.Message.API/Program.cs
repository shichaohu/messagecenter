using HS.Message.Extensions;
using HS.Message.HttpClients;
using HS.Message.Share.Authentication;
using HS.Message.Share.Extensions;
using HS.Message.Share.Handlers;
using HS.Message.Share.Log.Serilogs;
using HS.Message.Share.MessageEmitter;
using HS.Message.Share.MessageEmitter.Params;
using HS.Rabbitmq.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAspDotNetBasic();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.UseFileUpload(builder.WebHost);


builder.Services.AddControllers(option =>
{
    option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    option.Filters.Add(typeof(GlobalExceptionHandler));
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddAutoDependency("HS.Message");
builder.Services.AddRabbitmq("HS.Message.Service");

builder.Services.AddModelStateVrify();

builder.Services.AddSwaggerDoc(builder.Configuration);
builder.Services.AddCustomerHttpClient(builder.Configuration);
builder.Host.AddLogStrategy(builder.Logging, builder.Services, builder.Configuration);


var app = builder.Build();

app.UseLog(app.Environment);
app.UseCors("CorsPolicy");
app.UseSwaggerUi(builder.Configuration, app.Environment);
// 访问资源文件 文件位置放在 wwwroot 文件目录下
app.UseDefaultFiles().UseStaticFiles(new StaticFileOptions()
{
    // 设置 对应的文件类型（防止Mime type没事别出来，打不开或出现404错误）
    ServeUnknownFileTypes = true,

    // 设置默认 MIME TYPE
    DefaultContentType = "application/x-msdownload",

    // 设置支持的文件格式
    ContentTypeProvider = FileExtensions.SetSupportFile()
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseRabbitmq();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Logger.LogInformation($"The application started successfully, the following is information about the environment:{JsonConvert.SerializeObject(app.Environment)}");

app.Run();
