using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Data;
using BkpGasProcurementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BkpGasProcurementSystem.Areas.Identity.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BkpGasProcurementSystem.Views
{
    public class DeliveriesController : Controller
    {

        private readonly BkpGasProcurementSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<BkpGasProcurementSystemUser> _userManager;
        public DeliveriesController(BkpGasProcurementSystemContext context,UserManager<BkpGasProcurementSystemUser> usermgr,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = usermgr;
            _context = context;
            ViewData["cutomeruser"] = "";
            
        }
        public IActionResult getcurrentcustomer()
        {
            
            return ViewBag.currentcustomername;
        }
        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
            ViewData["customeruser"] = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
           
            ViewData["history_delivery"] = new List<update_delivery>();
           
            
            return View(await _context.Deliveries.ToListAsync());
        }

        // GET: Deliveries/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            _context.Deliveries.Include(m => m.delivery_history).SingleOrDefault(m => m.ID == id);
            var deliveries = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewData["delivery_history"] = deliveries.delivery_history;
           
            if (deliveries == null)
            {
                return NotFound();
            }

            return View(deliveries);
        }

        // GET: Deliveries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDelivery(List<Product> product, string phone, string address, string username, String price, DateTime ordertime, String paymentstat)
        {
            
            ViewBag.product = product;
            ViewBag.phone = phone;
            ViewBag.address = address;
            ViewBag.username = username;
            ViewBag.price = price;
            ViewBag.ordertime = ordertime;
            ViewBag.paymentstat = paymentstat;

            return View("~/Views/Deliveries/Create.cshtml");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] Deliveries deliveries, List<Product> product,string phone, string address, string username, string price,DateTime ordertime,String paymentstat)
        {
            
            
                
                deliveries.username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var npr = Regex.Match(price, @"\d+").Value;
            price = npr;
            //deliveries.orders = new Orders { phone = phone, address = address, username = username, total_price = float.Parse(price), order_date = ordertime, Payment_status = paymentstat, products = product };
                var update = new update_delivery { status = "In Delivery", update_when = DateTime.Now, message = "Assigned to Courier" };
                if (deliveries.delivery_history == null)
                {
                    deliveries.delivery_history = new List<update_delivery>();
                }
                deliveries.delivery_history.Add(update);
                deliveries.status = "Courier Assigned";
                deliveries.ship_time = DateTime.Now;
                
                _context.Add(deliveries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveries = await _context.Deliveries.FindAsync(id);
            if (deliveries == null)
            {
                return NotFound();
            }
            return View(deliveries);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] Deliveries deliveries, string message)
        {
            if (id != deliveries.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (deliveries.delivery_history == null)
                    {
                        deliveries.delivery_history = new List<update_delivery>();
                    }
                    if(message == "Delivery Completed")
                    {
                        deliveries.status = message;
                    }
                    deliveries.status = message;
                    
                    deliveries.delivery_history.Add(
                        
                        new update_delivery
                        {
                            message = message,
                            update_when = DateTime.Now,
                            status = "In Delivery"

                        });
                    deliveries.ship_time = DateTime.Now;
                    deliveries.username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    _context.Update(deliveries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveriesExists(deliveries.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deliveries);
        }
        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveries = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.ID == id);
            if (deliveries == null)
            {
                return NotFound();
            }

            return View(deliveries);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveries = await _context.Deliveries.FindAsync(id);
            _context.Deliveries.Remove(deliveries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveriesExists(int id)
        {
            return _context.Deliveries.Any(e => e.ID == id);
        }
    }
}
