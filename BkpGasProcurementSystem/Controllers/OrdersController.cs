using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Data;
using BkpGasProcurementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BkpGasProcurementSystem.Areas.Identity.Data;

namespace BkpGasProcurementSystem.Views
{
    public class OrdersController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<BkpGasProcurementSystemUser> _userManager;

        private readonly BkpGasProcurementSystemOrdersContext _context;

        public OrdersController(BkpGasProcurementSystemOrdersContext context, UserManager<BkpGasProcurementSystemUser> usermanager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = usermanager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {

            return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,order_date,username,address,phone,total_price,Payment_status")] Orders orders)
        {
            if (ModelState.IsValid)
            {

                var id = _userManager.GetUserId(HttpContext.User);
                orders.username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                BkpGasProcurementSystemUser user = _userManager.FindByIdAsync(id).Result;
                orders.address = user.Address;
                orders.phone = user.PhoneNumber;
                orders.order_date = DateTime.Now;
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var orderss = await _context.Orders.FindAsync(id);
                    _context.Orders.Remove(orderss);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return View("~/Views/Deliveries/Create.cshtml");
            }
            return View("~/Views/Deliveries/Create.cshtml");
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
