using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket.Models;

namespace Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketItemsController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketItemsController(TicketContext context)
        {
            _context = context;
           
        }

        // GET: api/TicketItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketItem>>> GetTicketItems()
        {
            if (_context.TicketItems.Count() == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    TicketItem t = new TicketItem
                    {
                        Price = 100
                    };
                    _context.TicketItems.Add(t);
                }
                await _context.SaveChangesAsync();
            }
            return await _context.TicketItems.ToListAsync();
        }

        // GET: api/TicketItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketItem>> GetTicketItem(int id)
        {
            
            var ticketItem = await _context.TicketItems.FindAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }

            return ticketItem;
        }

        // PUT: api/TicketItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketItem(int id, TicketItem ticketItem)
        {
            if (id != ticketItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketItemExists(id))
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

        // POST: api/TicketItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TicketItem>> PostTicketItem(TicketItem ticketItem)
        {
            _context.TicketItems.Add(ticketItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicketItem), new { id = ticketItem.Id }, ticketItem);
            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TicketItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TicketItem>> DeleteTicketItem(int id)
        {
            var ticketItem = await _context.TicketItems.FindAsync(id);
            if (ticketItem == null)
            {
                return NotFound();
            }

            _context.TicketItems.Remove(ticketItem);
            await _context.SaveChangesAsync();

            return ticketItem;
        }

        private bool TicketItemExists(int id)
        {
            return _context.TicketItems.Any(e => e.Id == id);
        }
    }
}
