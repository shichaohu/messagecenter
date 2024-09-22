using HS.Message.Model.Requests;
using HS.Message.Service.core;
using HS.Message.Service.tools;
using Microsoft.Extensions.Logging;

namespace XYH.Message.Server.Service
{
    /// <summary>
    ///  CheckTaskJob
    /// </summary>
    public class SmsQueryTaskJob : JobExectBase
    {
        private readonly AliyunSMSTool _aliyunSMSTool;
        private readonly ISmsMessageService _smsMessageService;
        private readonly Logger<SmsQueryTaskJob> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsQueryTaskJob(AliyunSMSTool aliyunSMSTool, ISmsMessageService smsMessageService,
            Logger<SmsQueryTaskJob> logger)
        {
            _aliyunSMSTool = aliyunSMSTool;
            _smsMessageService = smsMessageService;
            _logger = logger;
        }

        /// <summary>
        /// 开始执行方法体
        /// </summary>
        public override async void Execute()
        {
            try
            {
                var smsMessageList = await _smsMessageService.GetPageListAsync(
                    new HS.Message.Share.BaseModel.MPageQueryCondition<MSmsMessageCondtion>
                    {
                        pageIndex = 1,
                        pageSize = 100,
                        condition = new MSmsMessageCondtion
                        {
                            send_state = 1
                        }
                    });

                if (smsMessageList != null && smsMessageList?.Data?.Count > 0)
                {
                    var updateList = new List<Dictionary<string, object>>();
                    foreach (var smsMessage in smsMessageList.Data)
                    {
                        updateList.Add(new Dictionary<string, object>
                        {
                            { smsMessage.logical_id, 2 }
                        });
                    }
                    // 查询到数据后，就把数据查询状态锁定，避免其它任务扫描处理
                    await _smsMessageService.BactchUpdateDynamicAsync(updateList);

                    foreach (var model in smsMessageList.Data)
                    {
                        switch (model.channel_code)
                        {
                            case 1:
                                // 阿里云短信
                                await _aliyunSMSTool.QuerySendDetails(model);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"查询异常：{ex}");
            }
            finally
            {
            }
        }
    }
}
