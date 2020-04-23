using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Messages;
using Ticket.Models;

namespace Ticket.Controllers
{
    [Produces("application/json")]
    [Route("api/Booking")]
    public class BookingController : Controller
    {
        private readonly IBus _bus;
        private readonly TicketContext _context;

        public BookingController(IBus bus, TicketContext context)
        {
            //clientService = _clientService;
            _bus = bus;
            _context = context;
        }

        [HttpPost]
        public async Task<string> Post([FromBody]Booking booking)
        {
            var ticketItem = await _context.TicketItems.FindAsync(booking.TicketId);
            if (ticketItem == null)
            {
                return "there is no ticket id:"+ booking.TicketId;
            }
            if (!ticketItem.State.Equals("available"))
            {
                return "ticket " + booking.TicketId +" is not avaliable";
            }
            UserMessage message = new UserMessage
            {
                Id = booking.UserId,
                State = 0
            };
            await _bus.PublishAsync(message);

            int res= UserMessageConsumer.getRes();

            if (res == 1)
            {
                System.Console.WriteLine("user {0} exits", booking.UserId);
                //存在，发送消息给payment，锁定票
                UserTicketMessage userTicketMessage = new UserTicketMessage
                {
                    UserId = booking.UserId,
                    TicketId=booking.TicketId
                };
                await _bus.PublishAsync(userTicketMessage);
                ticketItem.State = "locked";
                _context.Entry(ticketItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                string mess = "user " + booking.UserId + " locked ticket " + booking.TicketId;
                return mess;
            }
            else
            {
                System.Console.WriteLine("user {0} does not exist", booking.UserId);
                string mess = "user "+ booking.UserId + " does not exist";
                return mess;
            }
            
           
        }
    }
}
