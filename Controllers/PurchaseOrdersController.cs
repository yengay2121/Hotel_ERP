using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelERP.Data;
using HotelERP.Models;

namespace HotelERP.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PurchaseOrders.Include(m => m.Hotel).Include(m => m.Supplier);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var purchaseorder = await _context.PurchaseOrders.Include(m => m.Hotel).Include(m => m.Supplier).FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseorder == null) return NotFound();
            return View(purchaseorder);
        }

        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SupplierId,HotelId,OrderDate,TotalAmount,Status")] PurchaseOrder purchaseorder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", purchaseorder?.HotelId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CompanyName", purchaseorder?.SupplierId);
            return View(purchaseorder);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var purchaseorder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseorder == null) return NotFound();
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", purchaseorder?.HotelId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CompanyName", purchaseorder?.SupplierId);
            return View(purchaseorder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierId,HotelId,OrderDate,TotalAmount,Status")] PurchaseOrder purchaseorder)
        {
            if (id != purchaseorder.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseorder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PurchaseOrders.Any(e => e.Id == purchaseorder.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", purchaseorder?.HotelId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CompanyName", purchaseorder?.SupplierId);
            return View(purchaseorder);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var purchaseorder = await _context.PurchaseOrders.Include(m => m.Hotel).Include(m => m.Supplier).FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseorder == null) return NotFound();
            return View(purchaseorder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseorder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseorder != null) _context.PurchaseOrders.Remove(purchaseorder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}