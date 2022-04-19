using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Models;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace SubscriptionManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static User user = new User();
        private readonly SubscriptionManagerContext _context;

        public UsersController(SubscriptionManagerContext context)
        {
            _context = context;
        }

        // GET: api/Users/5/Subscription/3
        [HttpGet("{user_id}/Subscription/{sub_id}")]
        public async Task<ActionResult<Subscription>> GetSubscriptionFromUser(int user_id, int sub_id)
        {
            var user = await _context.User.FindAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            Subscription subscription = user.GetSubscriptionByID(sub_id);
            if (subscription == null)
                return NotFound();
            return subscription;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
