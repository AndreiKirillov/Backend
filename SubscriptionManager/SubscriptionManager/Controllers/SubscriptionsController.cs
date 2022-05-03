using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Models;
using SubscriptionManager.Services;
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
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

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
            if (subs_list == null)
                return NotFound();

            var subscription = subs_list.Where(s => s.Id == id).FirstOrDefault();

            if (subscription == null)
                return NotFound();

            return Ok(subscription);
        }

        // GET: api/Subscriptions/nearest
        [Authorize]
        [HttpGet("nearest")]       // запрос на конкретную подписку юзера
        public async Task<ActionResult<Subscription>> GetNearest()
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            var subs_list = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();

            if (subs_list == null)
                return NotFound();

            var subscription = SubscriptionService.GetNearestSubscription(subs_list);

            if (subscription == null)
                return NotFound();

            return Ok(subscription);
        }

        // GET: api/Subscriptions/debt
        [Authorize]
        [HttpGet("debt")]       // запрос на получение просроченных подписок
        public async Task<ActionResult<Subscription>> GetDebt()
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            var subs_list = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();

            if (subs_list == null)
                return NotFound();

            var debt_subs = SubscriptionService.GetDebtSubscriptions(subs_list);

            if (debt_subs == null)
                return NotFound();

            return Ok(debt_subs);
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

            subscription.PaymentDate.AddMonths(1); // рассчитываем дату следующего платежа

            user.Subs.Add(subscription);

            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok(subscription);
        }

        // PUT: api/Subscriptions/pay/5
        [Authorize]
        [HttpPut("pay/{id}")]            // Метод оплаты подписки
        public async Task<IActionResult> MakePayment(int id)
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            var subs_list = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();
            if (subs_list == null)
                return NotFound();

            var subscription = subs_list.Where(s => s.Id == id).FirstOrDefault();
            if (subscription == null)
                return NotFound();

            SubscriptionService.RefreshSubscription(ref subscription);      // обновляем данные

            _context.Entry(subscription).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Subscriptions/AddCategory/5
        [Authorize]
        [HttpPut("AddCategory/{id}&&{category_id}")]            // добавление категории к подписке
        public async Task<IActionResult> AddCategory(int id, int category_id)
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            // ищем подписку
            var subs_list = await _context.Subscription.Where(s => s.UserID == user.Id).ToListAsync();
            if (subs_list == null)
                return NotFound();

            var subscription = subs_list.Where(s => s.Id == id).FirstOrDefault();
            if (subscription == null)
                return NotFound();

            // ищем категорию
            var category = _context.Category.Where(c => c.Id == category_id).FirstOrDefault();
            if (category == null)
                return BadRequest();

            subscription._Category = category;

            _context.Entry(subscription).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
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
