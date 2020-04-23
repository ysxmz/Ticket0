using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Messages;

namespace User.Models
{
    public class UserMessageConsumer : IConsumeAsync<UserMessage>
    {
        [AutoSubscriberConsumer(SubscriptionId = "UserMessage.Notice")]
        public Task ConsumeAsync(UserMessage message)
        {
            // Your business logic code here
            // eg.Build one email to client via SMTP service
            // Sample console code
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.WriteLine("Consume one message from RabbitMQ : judge whether the user {0} exists", message.Id);
            System.Console.ResetColor();
            IServiceProvider service = ServiceLocator.Instance;
            UserContext _context = service.CreateScope().ServiceProvider.GetService<UserContext>();
            var userItem = _context.UserItems.Find(message.Id);
            
            IBus _bus = service.CreateScope().ServiceProvider.GetService<IBus>();
            UserMessageReply messageReply = new UserMessageReply
            {
                Id = message.Id
            };
            if (userItem == null)
            {
                System.Console.WriteLine("user {0} does not exist", message.Id);
                messageReply.State = 0;
            }
            else
            {
                System.Console.WriteLine("user {0} exist", message.Id);
                messageReply.State = 1;
            }
            _bus.PublishAsync(messageReply);
            return Task.CompletedTask;
        }
    }
}
