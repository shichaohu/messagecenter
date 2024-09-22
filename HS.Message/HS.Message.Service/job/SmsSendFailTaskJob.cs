namespace XYH.Message.Server.Service
{
    /// <summary>
    ///  短信查询锁定解锁任务
    /// </summary>
    public class SmsSendFailTaskJob : JobExectBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsSendFailTaskJob()
        {

        }

        /// <summary>
        /// 开始执行方法体
        /// </summary>
        public override void Execute()
        {
            try
            {
                // 直接更新解锁
                //new SmsMessageDetailsRepository().SendFailOpert();
            }
            catch (Exception ex)
            {
                //XYHLogOperator.WriteLog("短信发送失败超时异常：", ex);
            }
            finally
            {
            }
        }
    }
}
