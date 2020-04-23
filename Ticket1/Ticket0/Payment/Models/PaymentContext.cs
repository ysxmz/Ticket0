using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Models
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public DbSet<PaymentItem> PaymentItems { get; set; }
    
    }
}
