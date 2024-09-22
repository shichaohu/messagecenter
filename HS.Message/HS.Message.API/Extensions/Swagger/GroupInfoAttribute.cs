namespace HS.Message.Extensions.Swagger
{
    /// <summary>
    /// 分组的描述信息
    /// </summary>
    public class GroupInfoAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }
    }
}
