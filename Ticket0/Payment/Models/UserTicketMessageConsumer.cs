using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Models
{
    public class UserTicketMessageConsumer : IConsumeAsync<UserTicketMessage>
    {
        [AutoSubscriberConsumer(SubscriptionId = "UserTicketMessage.Notice")]
        public Task ConsumeAsync(UserTicketMessage message)
        {
            System.Console.WriteLine("user {0} locked ticket {1}", message.UserId, message.TicketId);
            IServiceProvider service = ServiceLocator.Instance;
            PaymentContext _context = service.CreateScope().ServiceProvider.GetService<PaymentContext>();
            PaymentItem paymentItem = new PaymentItem
            {
                UserId = message.UserId,
                TicketId = message.TicketId,
                State = "unpaid",
                CreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                PayDate =null
            };
            _context.PaymentItems.Add(paymentItem);
            _context.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
