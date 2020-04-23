using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Messages
{
    public class UserTicketMessage
    {
        public int UserId { get; set; }
        public int TicketId { get; set; }
    }
}
