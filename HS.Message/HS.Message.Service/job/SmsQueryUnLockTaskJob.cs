namespace XYH.Message.Server.Service
{
    /// <summary>
    ///  短信查询锁定解锁任务
    /// </summary>
    public class SmsQueryUnLockTaskJob : JobExectBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsQueryUnLockTaskJob()
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
                //new SmsMessageDetailsRepository().QueryUnLockState();
            }
            catch (Exception ex)
            {
                //XYHLogOperator.WriteLog("短信结果查询解锁异常：", ex);
            }
            finally
            {
            }
        }
    }
}
