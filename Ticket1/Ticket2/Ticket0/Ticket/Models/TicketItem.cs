using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Models
{
    public class TicketItem
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string State { get; set; } = "available";
    }
}
