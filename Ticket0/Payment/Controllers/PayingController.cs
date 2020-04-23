using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.Models;
using RabbitMQ.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Controllers
{
    [Produces("application/json")]
    [Route("api/Paying")]
    public class PayingController : Controller
    {
        private readonly IBus _bus;
        private readonly PaymentContext _context;

        public PayingController(IBus bus, PaymentContext context)
        {
            //clientService = _clientService;
            _bus = bus;
            _context = context;
        }

        [HttpPost]
        public async Task<string> Post([FromBody]Paying paying)
        {
            var paymentItem = await _context.PaymentItems.FindAsync(paying.Id);
            if (paymentItem == null)
            {
                return "there is no payment id:" + paying.Id;
            }

            if (paying.opertion.Equals("pay"))
            {
                if (!paymentItem.State.Equals("unpaid"))
                {
                    return "payment id " + paying.Id + " is not unpaid";
                }
                paymentItem.State = "paid";
                paymentItem.PayDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                _context.Entry(paymentItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                //发布一个信息，告诉Ticket去更新票的状态
                TicketMessage ticketMessage = new TicketMessage
                {
                    TicketId = paymentItem.TicketId,
                    State = 1

                };
                await _bus.PublishAsync(ticketMessage);
                string mess = "payment id " + paying.Id + " is paid and ticket " + paymentItem.TicketId + " is sold now";
                return mess;
            }else if (paying.opertion.Equals("cancel"))
            {
                if (!paymentItem.State.Equals("unpaid"))
                {
                    return "payment id " + paying.Id + " is not unpaid";
                }
                paymentItem.State = "cancelled";
                _context.Entry(paymentItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                //发布一个信息，告诉Ticket去更新票的状态
                TicketMessage ticketMessage = new TicketMessage
                {
                    TicketId = paymentItem.TicketId,
                    State = 2
                };
                await _bus.PublishAsync(ticketMessage);
                string mess = "payment id " + paying.Id + " is cancelled and ticket " + paymentItem.TicketId + " is available now";
                return mess;

            }
            else if (paying.opertion.Equals("refund"))
            {
                if (!paymentItem.State.Equals("paid"))
                {
                    return "payment id " + paying.Id + " is not paid";
                }
                
                paymentItem.State = "refund";
                _context.Entry(paymentItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                //发布一个信息，告诉Ticket去更新票的状态
                TicketMessage ticketMessage = new TicketMessage
                {
                    TicketId = paymentItem.TicketId,
                    State = 2
                };
                await _bus.PublishAsync(ticketMessage);
                string mess = "payment id " + paying.Id + " is refund and ticket " + paymentItem.TicketId + " is available now";
                return mess;
            }
            else
            {
                string mess = "opertion should be pay, cancel or refund ";
                return mess;
            }

        }
    }
}
