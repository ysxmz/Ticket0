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
        [AutoSubscriberConsumer(SubscriptionId = "TicketMessage.Notice")]
        public Task ConsumeAsync(TicketMessage message)
        {
            IServiceProvider service = ServiceLocator.Instance;
            TicketContext _context = service.CreateScope().ServiceProvider.GetService<TicketContext>();
            
            var ticketItem = _context.TicketItems.Find(message.TicketId);
            if (message.State == 1)
            {
                System.Console.WriteLine("ticket {0} sold", message.TicketId);
                ticketItem.State = "sold";

            }
            else if(message.State == 2)
            {
                System.Console.WriteLine("ticket {0} is available", message.TicketId);
                ticketItem.State = "available";
            }
            _context.Entry(ticketItem).State = EntityState.Modified;
            _context.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
