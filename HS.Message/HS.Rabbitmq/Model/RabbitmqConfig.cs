using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Rabbitmq.Model
{
    public class RabbitmqConfig
    {
        public string[] Channels { get; set; }
        public string Prefix { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string NameSpace { get; set; }
    }
}
