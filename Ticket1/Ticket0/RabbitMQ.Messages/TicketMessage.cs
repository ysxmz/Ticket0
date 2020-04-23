using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Messages
{
    public class TicketMessage
    {
        public int TicketId { get; set; }
        public int State { get; set; } //1已支付，更新状态为sold; 2已取消，更新状态为available
    }
}
