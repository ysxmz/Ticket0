using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.Models;

namespace Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentItemsController : ControllerBase
    {
        private readonly PaymentContext _context;

        public PaymentItemsController(PaymentContext context)
        {
            _context = context;
        }

        // GET: api/PaymentItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentItem>>> GetPaymentItems()
        {
            return await _context.PaymentItems.ToListAsync();
        }

        // GET: api/PaymentItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentItem>> GetPaymentItem(int id)
        {
            var paymentItem = await _context.PaymentItems.FindAsync(id);

            if (paymentItem == null)
            {
                return NotFound();
            }

            return paymentItem;
        }

        // PUT: api/PaymentItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentItem(int id, PaymentItem paymentItem)
        {
            if (id != paymentItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PaymentItem>> PostPaymentItem(PaymentItem paymentItem)
        {
            _context.PaymentItems.Add(paymentItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentItem), new { id = paymentItem.Id }, paymentItem);
        }

        // DELETE: api/PaymentItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentItem>> DeletePaymentItem(int id)
        {
            var paymentItem = await _context.PaymentItems.FindAsync(id);
            if (paymentItem == null)
            {
                return NotFound();
            }

            _context.PaymentItems.Remove(paymentItem);
            await _context.SaveChangesAsync();

            return paymentItem;
        }

        private bool PaymentItemExists(int id)
        {
            return _context.PaymentItems.Any(e => e.Id == id);
        }
    }
}
