using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;
namespace RabbitMQ.Messages
{
    [Queue("Qka.UserReply", ExchangeName = "Qka.UserReply")]
    public class UserMessageReply
    {
        public int Id { get; set; }
        public int State { get; set; }//1存在 2不存在
    }
}
