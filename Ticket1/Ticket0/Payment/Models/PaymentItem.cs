using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Models
{
    public class PaymentItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public String State { get; set; } //未支付，已支付，退款，已取消
        public String CreateDate { get; set; }
        public String PayDate { get; set; }

    }
}
