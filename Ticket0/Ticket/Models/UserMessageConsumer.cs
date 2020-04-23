using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using RabbitMQ.Messages;

namespace Ticket.Models
{
    public class UserMessageConsumer : IConsumeAsync<UserMessageReply>
    {
        public static UserMessageReply messageReply = new UserMessageReply
        {
            Id = -1,
            State = -1
        };

        [AutoSubscriberConsumer(SubscriptionId = "UserMessageReply.Notice")]
        public Task ConsumeAsync(UserMessageReply message)
        {
            messageReply.Id = message.Id; 
            messageReply.State = message.State;
           
            if (message.State == 1)
            {
                System.Console.WriteLine("user {0} exits", message.Id);
            }
            else
            {
                System.Console.WriteLine("user {0} does not exist", message.Id);
            }
            
            return Task.CompletedTask;
        }

        internal static int getRes()
        {
            while (messageReply.State == -1)
            {

            }
            int state = messageReply.State;
            messageReply.State = -1;
            return state;
        }
    }
}
