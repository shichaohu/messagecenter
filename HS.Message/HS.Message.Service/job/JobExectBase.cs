using HS.Message.Model.domain;
using Quartz;
using System.Threading.Tasks;

namespace XYH.Message.Server.Service
{
    /// <summary>
    /// 定时任务执行基类
    /// </summary>
    public abstract class JobExectBase : IJob
    {
        /// <summary>
        /// 听取头 token
        /// </summary>
        public static List<MHeadParamet> headerKeyValue = new List<MHeadParamet>(){new MHeadParamet
        {
            key = "token",
        } };

        /// <summary>
        /// 执行的任务id
        /// </summary>
        public int job_id;

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobExectBase()
        {
        }

        /// <summary>
        /// 具体执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            // 获取执行的任务id
            job_id = context.JobDetail.JobDataMap.GetInt("job_id");

            await Start();
        }

        /// <summary>
        /// 开始执行方法体
        /// </summary>
        public async Task Start()
        {
            try
            {
                // 执行具体的业务逻辑，各个类自己去实现
                Execute();

            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        /// <summary>
        /// 异步调度具体执行业务逻辑
        /// </summary>
        public abstract void Execute();
    }
}
