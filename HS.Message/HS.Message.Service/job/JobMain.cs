using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace XYH.Message.Server.Service
{
    /// <summary>
    /// job入口
    /// </summary>
    public class JobMain
    {
        /// <summary>
        ///  调度器
        /// </summary>
        private IScheduler scheduler;

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await StartJob();
        }

        /// <summary>
        /// 创建调度任务的公共调用中心
        /// </summary>
        /// <returns></returns>
        public async Task StartJob()
        {

            //创建一个工厂
            NameValueCollection param = new NameValueCollection()
            {
                {  "timingTask","mseeage"}
            };

            // 调度工厂
            StdSchedulerFactory factory = new StdSchedulerFactory(param);

            //创建一个调度器
            scheduler = await factory.GetScheduler();

            //开始调度器
            await scheduler.Start();

            // 创建一个短信发送结果查询定时任务
            // 每10s执行一次
            CreateJob<SmsQueryTaskJob>("", "SmsQueryTaskJob", "0/10 * * * * ? ", 1);

            // 创建一个短信查询解锁任务
            // 每5分钟执行一次
            CreateJob<SmsQueryUnLockTaskJob>("", "SmsQueryUnLockTaskJob", "0 0/5 * * * ? ", 1);

            // 创建一个短信超时发送失败任务
            // 每5分钟执行一次
            CreateJob<SmsSendFailTaskJob>("", "SmsSendFailTaskJob", "0 0/5 * * * ? ", 1);
        }

        /// <summary>
        /// 创建运行的调度器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <param name="cronTime"></param>
        /// <returns></returns>
        public async Task CreateJob<T>(string name, string group, string cronTime, int job_id) where T : IJob
        {
            //创建一个作业
            var job = JobBuilder.Create<T>()
                .WithIdentity("name" + name, "gtoup" + group)
                .Build();

            //创建一个触发器
            var tigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("name" + name, "group" + group)
                .StartNow()
                .WithCronSchedule(cronTime)
                .Build();

            job.JobDataMap.Add("job_id", job_id);

            //把作业和触发器放入调度器中
            await scheduler.ScheduleJob(job, tigger);
        }

        /// <summary>
        /// 停止调度器            
        /// </summary>
        public void Stop()
        {
            scheduler.Shutdown();
            scheduler = null;
        }

        ///// <summary>
        ///// job启动
        ///// </summary>
        //public void Start()
        //    {


        //        //try
        //        //{
        //        //    // 获取开启的定时任务
        //        //    List<MJobConfig> jobConfigList = new JobConfigRepository().GetAllList(new MJobConfig()
        //        //    {
        //        //        enable = 1
        //        //    });

        //        //    if (jobConfigList != null && jobConfigList.Count > 0)
        //        //    {
        //        //        foreach (var jobConfig in jobConfigList)
        //        //        {
        //        //            switch (jobConfig.code)
        //        //            {
        //        //                case EJobConfigCode.systemDataCheck:
        //        //                    // 系统自动定时数据盘点处理
        //        //                    RecurringJob.AddOrUpdate(() => new SystemDataCheckJob().Start(), jobConfig.exect_rule_cron);
        //        //                    break;
        //        //                case EJobConfigCode.taskLogDelete:
        //        //                    // 任务日志删除任务
        //        //                    RecurringJob.AddOrUpdate(() => new TaskLogDeleteJob().Start(), jobConfig.exect_rule_cron);
        //        //                    break;
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    XYHLogOperator.WriteLog($"Job启动,系统异常:{ex.Message};{ex.StackTrace}", "JobMain.Start", null, null, LogLevel.Error);
        //        //}
        //    }
    }
}
