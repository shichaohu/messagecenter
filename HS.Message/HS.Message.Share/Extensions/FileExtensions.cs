using Microsoft.AspNetCore.StaticFiles;

namespace HS.Message.Share.Extensions
{
    public static class FileExtensions
    {
        public static FileExtensionContentTypeProvider SetSupportFile()
        {
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            fileExtensionContentTypeProvider.Mappings[".xls"] = "application/vnd.ms-excel";
            fileExtensionContentTypeProvider.Mappings[".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return fileExtensionContentTypeProvider;
        }
    }
}
