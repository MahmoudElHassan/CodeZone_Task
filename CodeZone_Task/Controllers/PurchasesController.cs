using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeZone_Task.Models;

namespace CodeZone_Task.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Purchases.Include(p => p.Items).Include(p => p.Stores);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Items)
                .Include(p => p.Stores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["Item_Id"] = new SelectList(_context.Items, "Id", "ItemName");
            ViewData["Store_Id"] = new SelectList(_context.Stores, "Id", "StoreName");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Store_Id,Item_Id,Stock")] Purchase purchase)
        {
            var forigenKeysDB = _context.Purchases.Where(x => x.Item_Id == purchase.Item_Id
            && x.Store_Id == purchase.Store_Id).ToList();

            if (ModelState.IsValid)
            {
                foreach (var product in forigenKeysDB)
                {
                    if (product.Item_Id == purchase.Item_Id &&
                        product.Store_Id == purchase.Store_Id)
                    {
                        ViewBag.Message = string.Format("This Purchase already Exists",
                            DateTime.Now.ToString());
                        return View();
                    }
                }

                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Item_Id"] = new SelectList(_context.Items, "Id", "ItemName", purchase.Item_Id);
            ViewData["Store_Id"] = new SelectList(_context.Stores, "Id", "StoreName", purchase.Store_Id);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["Item_Id"] = new SelectList(_context.Items, "Id", "ItemName", purchase.Item_Id);
            ViewData["Store_Id"] = new SelectList(_context.Stores, "Id", "StoreName", purchase.Store_Id);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Store_Id,Item_Id,Stock,New_Stock")] Purchase purchase)
        {
            var purchaseDB = await _context.Purchases.FindAsync(id);

            if (purchaseDB == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var forigenKeysDB = _context.Purchases.Where(x => x.Item_Id == purchase.Item_Id
                        && x.Store_Id == purchase.Store_Id).ToList();

                    if (ModelState.IsValid)
                    {
                        if (purchaseDB.Item_Id != purchase.Item_Id ||
                            purchaseDB.Store_Id != purchase.Store_Id)
                        {
                            foreach (var product in forigenKeysDB)
                            {
                                if (product.Item_Id == purchase.Item_Id &&
                                    product.Store_Id == purchase.Store_Id)
                                {
                                    ViewBag.Message = string.Format("This Purchase already Exists",
                                        DateTime.Now.ToString());
                                    return View();
                                }
                            }

                            var newDB = new Purchase()
                            {
                                Store_Id = purchase.Store_Id,
                                Item_Id = purchase.Item_Id,
                                Stock = purchase.New_Stock
                            };

                            _context.Add(newDB);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }

                        purchaseDB.Item_Id = purchase.Item_Id;
                        purchaseDB.Store_Id = purchase.Store_Id;
                        purchaseDB.Stock += purchase.New_Stock;

                        _context.Update(purchaseDB);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Id))
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
            //ViewData["Item_Id"] = new SelectList(_context.Items, "Id", "ItemName", purchase.Item_Id);
            //ViewData["Store_Id"] = new SelectList(_context.Stores, "Id", "StoreName", purchase.Store_Id);
            ViewData["New_Stock"] = new SelectList(_context.Stores, "Id", "New_Stock", purchase.New_Stock);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Items)
                .Include(p => p.Stores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }
    }
}
