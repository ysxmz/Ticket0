using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Messages;

namespace Ticket.Models
{
    public class TicketMessageConsumer:IConsumeAsync<TicketMessage>
    {
        public static int paymentId = -1;
        [AutoSubscriberConsumer(SubscriptionId = "TicketMessage.Notice")]
        public Task ConsumeAsync(TicketMessage message)
        {
            IServiceProvider service = ServiceLocator.Instance;
            TicketContext _context = service.CreateScope().ServiceProvider.GetService<TicketContext>();
            
            var ticketItem = _context.TicketItems.Find(message.TicketId);
            if (message.State == 1)
            {
                System.Console.WriteLine("ticket {0} is sold", message.TicketId);
                ticketItem.State = "sold";
                _context.Entry(ticketItem).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            else if(message.State == 2)
            {
                System.Console.WriteLine("ticket {0} is available", message.TicketId);
                ticketItem.State = "available";
                _context.Entry(ticketItem).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            else if (message.State == 3)
            {
                System.Console.WriteLine("payment id is {0}", message.TicketId);
                paymentId = message.TicketId;
            }
            
            return Task.CompletedTask;
        }
        public static int getPaymentId()
        {
            while (paymentId == -1)
            {

            }
            int res = paymentId;
            paymentId = -1;
            return res;
        }
    }
}
