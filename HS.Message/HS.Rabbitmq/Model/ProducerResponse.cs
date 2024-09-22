using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Rabbitmq.Model
{
    public class ProducerResponse
    {

        /// <summary>
        /// 是否消费成功
        /// </summary>
        public bool Successed { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
    }
}
