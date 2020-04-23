using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;
namespace RabbitMQ.Messages
{
    [Queue("Qka.User", ExchangeName = "Qka.User")]
    public class UserMessage
    {
        public int Id { get; set; }
        public int State { get; set; }//1存在 0不存在
    }
}
