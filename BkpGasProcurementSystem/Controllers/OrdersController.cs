﻿using System;
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
 
        public async Task<IActionResult> Create(int ordid,[Bind("ID,order_date,username,address,phone,total_price,Payment_status")] Orders orders, string price, string type, string name, byte[] pic, int weight)
        {
            if (ModelState.IsValid)
            {

                var id = _userManager.GetUserId(HttpContext.User);
                orders.username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                BkpGasProcurementSystemUser user = _userManager.FindByIdAsync(id).Result;
                _context.Orders.Include(m => m.products).SingleOrDefault(m => m.username == id);
                var order_list = _context.Orders;
                var flag = false;
            Orders unpaid_order = null;
                if (order_list.Count() > 0)
                {
                    unpaid_order = order_list.Where(o => o.username == user.UserName)
                        .Where(o => o.Payment_status.ToUpper() == "PENDING")
                        .FirstOrDefault();
                }

                price = price.Remove(0, 1);
                int len = price.Length-3;
                price = price.Remove(len, 3);
                if(unpaid_order != null)
                {
                    flag = true;
                }
                

                Product first_item = new Product
                {
                    Name = name,
                    Weight = weight,
                    Picture = pic,
                    
                    Price = int.Parse(price),
                    Type = type

                };
                if (unpaid_order == null)
                {
                    unpaid_order = new Orders
                    {
                        products = new List<Product>(),
                        username = user.UserName,
                        Payment_status = "pending".ToUpper(),
                        address = user.Address,
                        phone = user.PhoneNumber,
                        order_date = DateTime.Now,
                        total_price = 0.0f
                    };
                }
                var tot = 0;
                foreach( var a in unpaid_order.products)
                {
                    tot  = tot + a.Price;
                }
                unpaid_order.total_price = tot + int.Parse(price);

            System.Diagnostics.Debug.WriteLine(unpaid_order.ID);




                if(unpaid_order.ID == ordid)
                {
                    unpaid_order.ID = ordid;
                }
                
                unpaid_order.products.Add(first_item);
                orders.address = user.Address;
                orders.phone = user.PhoneNumber;
                if(flag == true)
                {
                    System.Diagnostics.Debug.WriteLine(unpaid_order.ID);
                    _context.Orders.Update(unpaid_order);
                }
                else
                {
                    _context.Orders.Add(unpaid_order);
                }
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
