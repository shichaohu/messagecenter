namespace HS.Message.Extensions.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public enum ApiGroupNames
    {
        /// <summary>
        /// All 所有
        /// </summary>
        [GroupInfo(Title = "All 所有", Description = "All接口", Version = "")]
        All = 0,
        /// <summary>
        /// NoGroup 未分组
        /// </summary>
        [GroupInfo(Title = "NoGroup 未分组", Description = "尚未分组的接口", Version = "")]
        NoGroup,
        /// <summary>
        /// 系统设置
        /// </summary>
        [GroupInfo(Title = "系统设置", Description = "系统设置", Version = "")]
        SystemSetting,
        /// <summary>
        /// 消息中心
        /// </summary>
        [GroupInfo(Title = "消息中心", Description = "消息中心", Version = "")]
        Message,
        /// <summary>
        /// Developing 开发中
        /// </summary>
        [GroupInfo(Title = "Developing 开发中", Description = "开发中", Version = "")]
        Developing
    }
}
