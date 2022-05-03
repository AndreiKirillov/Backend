﻿using System;
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
        [HttpGet]            // Получение всех подписок юзера
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscription()
        {
            var user = _context.User.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            var subs = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();

            if (subs == null)
                return NotFound();

            return Ok(subs);
        }

        // GET: api/Subscriptions/5
        [Authorize]
        [HttpGet("{id}")]       // запрос на конкретную подписку юзера
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            var subs_list = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();

            var subscription = subs_list.Where(s => s.Id == id).FirstOrDefault();


            if (subscription == null)
                return NotFound();

            return Ok(subscription);
        }

        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
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

        // POST: api/Subscriptions/add
        [Authorize]
        [HttpPost("add")]      // Добавление подписки 
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
            if (subscription.ServiceName == null)
                return BadRequest("Not enough data in request!");

            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            _context.Category.IgnoreAutoIncludes();

            subscription.UserID = user.Id;

            user.Subs.Add(subscription);

            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok(subscription);
        }

        // DELETE: api/Subscriptions/delete/5
        [Authorize]
        [HttpDelete("delete/{id}")]           // Удаление подписки
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            var subs_list = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();

            var subscription = subs_list.Where(s => s.Id == id).FirstOrDefault();

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
