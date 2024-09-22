using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Rabbitmq.Model
{
    public class ConsumerResponse
    {
        /// <summary>
        /// 是否消费成功
        /// </summary>
        public bool Successed { get; set; }
        /// <summary>
        /// 消费失败时的延迟时间（秒）
        /// </summary>
        public int DelaySeconds { get; set; }
    }
}
