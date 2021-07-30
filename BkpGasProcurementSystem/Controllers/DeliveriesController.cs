using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Data;
using BkpGasProcurementSystem.Models;

namespace BkpGasProcurementSystem.Views
{
    public class DeliveriesController : Controller
    {
        private readonly BkpGasProcurementSystemDeliveriesContext _context;

        public DeliveriesController(BkpGasProcurementSystemDeliveriesContext context)
        {
            _context = context;
        }

        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deliveries.ToListAsync());
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> Create([Bind("ID,status,ship_time")] Deliveries deliveries)
        {
            if (ModelState.IsValid)
            {
                var update = new update_delivery { status = "In Delivery", update_when = DateTime.Now, message = "Assigned to Courier" };
                if (deliveries.delivery_history == null)
                {
                    deliveries.delivery_history = new List<update_delivery>();
                }
                deliveries.delivery_history.Add(update);
                _context.Add(deliveries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveries);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,status,ship_time")] Deliveries deliveries)
        {
            if (id != deliveries.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
