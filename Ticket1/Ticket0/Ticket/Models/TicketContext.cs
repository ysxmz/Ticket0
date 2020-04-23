using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Models
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options)
            : base(options)
        {
        }

        public DbSet<TicketItem> TicketItems { get; set; }
    }
}
