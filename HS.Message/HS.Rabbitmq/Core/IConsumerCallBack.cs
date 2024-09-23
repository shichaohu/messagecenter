using HS.Message.Share.BaseModel;
using HS.Rabbitmq.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Rabbitmq.Core
{
    public interface IConsumerCallBack
    {
        Task<BaseResponse<ConsumerResponse>> RunAsync(QueueMessage message);
    }
}
