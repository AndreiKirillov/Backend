using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace SubscriptionManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionManagerContext _context;

        public SubscriptionsController(SubscriptionManagerContext context)
        {
            _context = context;
        }

        // GET: api/Subscriptions
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscription()
        {
            //var query =
            //    from user in _context.User.ToList()
            //    join subs in _context.Subscription.ToList()
            //    on user.Subs equals subs.Id;


            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();


            return  user.Subs.ToList();
        }

        // GET: api/Subscriptions/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var subscription = await _context.Subscription.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
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

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
            if (subscription.ServiceName == null)
                return BadRequest("Not enough data in request!");

            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            _context.Category.IgnoreAutoIncludes();

            user.Subs.Add(subscription);

            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok(subscription);
        }

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscription.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscription.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscription.Any(e => e.Id == id);
        }
    }
}
