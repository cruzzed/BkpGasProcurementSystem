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
using System.IO;
using System.Text;
using BkpGasProcurementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace BkpGasProcurementSystem.Views
{
    public class ProductsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<BkpGasProcurementSystemUser> _userManager;
        private readonly BkpGasProcurementSystemProductContext _context;
        private readonly BkpGasProcurementSystemOrdersContext _order_context;

        public ProductsController(
            BkpGasProcurementSystemProductContext context,
            BkpGasProcurementSystemOrdersContext order_context,
            UserManager<BkpGasProcurementSystemUser> userManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
            _order_context = order_context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Price,Weight")] Product product, 
                                                IFormFile Picture)
        {

            if (ModelState.IsValid)
            {
                
                // check if picture is valid
                var PictureValid = ValidatePicture(Picture);
                if (PictureValid.GetType() != typeof(OkResult)) return PictureValid;

                await LoadFormPictureToProduct(product, Picture);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Price,Weight")] Product product,
                                              IFormFile Picture)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // check if picture is valid
                    var PictureValid = ValidatePicture(Picture);
                    if (PictureValid.GetType() != typeof(OkResult)) return PictureValid;

                    await LoadFormPictureToProduct(product, Picture);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public IActionResult ValidatePicture(IFormFile Picture)
        {
            var MAX_SIZE = 1 * 1024 * 1024;
            if (Picture != null)
            {
                if (Picture.ContentType.ToLower().Split("/")[0] != "image") //accept=".png,.jpg,.jpeg,.gif,.tif"
                {
                    return BadRequest("The " + Picture.FileName +
                        " unable to upload because uploaded file must be a text file");
                }
                if (Picture.Length == 0)
                {
                    return BadRequest("The " + Picture.FileName + "file is empty content!");
                }
                else if (Picture.Length > MAX_SIZE)
                {
                    return BadRequest("The " + Picture.FileName + "file is exceed 1 MB !");
                }
                return Ok();
            }
            return BadRequest("Unknown Error!");
        }

        public async Task<Product> LoadFormPictureToProduct(Product P, IFormFile Picture)
        {
            using (var stream = new MemoryStream())
            {
                await Picture.CopyToAsync(stream);
                P.Picture = stream.ToArray();
            }
            return P;
        }

        [HttpPost, ActionName("AddToOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToOrder([Bind("Id")] Product product)
        {
            BkpGasProcurementSystemUser user = _userManager.FindByIdAsync(
                _userManager.GetUserId(HttpContext.User)
            ).Result;

            var order_list = from order in _order_context.Orders
                             select order;

            Orders unpaid_order = null;

            if (order_list.Count() > 0) { 
                unpaid_order = order_list.Where(o => o.Payment_status.ToLower() == "pending")
                                         .Where(o => o.username == user.UserName)
                                         .First();
            }

            if (unpaid_order == null) {
                Debug.WriteLine("im null!");
                unpaid_order = new Orders
                {
                    products = new List<Product>(),
                    username = user.UserName,
                    //Payment_status = "pending".ToLower(),
                    //address = user.Address,
                    //phone = user.PhoneNumber,
                    //order_date = DateTime.Now,
                    //total_price = 0.0f
                };
                _order_context.Add(unpaid_order);
            }

            Debug.WriteLine(unpaid_order.ID);

            unpaid_order.products.Add(product);
            _order_context.Add(unpaid_order);

            
            return RedirectToAction(nameof(Index));
        }


    }
}
