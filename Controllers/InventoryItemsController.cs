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
    public class InventoryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.InventoryItems.Include(m => m.Hotel);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var inventoryitem = await _context.InventoryItems.Include(m => m.Hotel).FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryitem == null) return NotFound();
            return View(inventoryitem);
        }

        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SKU,QuantityInStock,UnitPrice,HotelId")] InventoryItem inventoryitem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryitem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", inventoryitem?.HotelId);
            return View(inventoryitem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var inventoryitem = await _context.InventoryItems.FindAsync(id);
            if (inventoryitem == null) return NotFound();
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", inventoryitem?.HotelId);
            return View(inventoryitem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SKU,QuantityInStock,UnitPrice,HotelId")] InventoryItem inventoryitem)
        {
            if (id != inventoryitem.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.InventoryItems.Any(e => e.Id == inventoryitem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", inventoryitem?.HotelId);
            return View(inventoryitem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var inventoryitem = await _context.InventoryItems.Include(m => m.Hotel).FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryitem == null) return NotFound();
            return View(inventoryitem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryitem = await _context.InventoryItems.FindAsync(id);
            if (inventoryitem != null) _context.InventoryItems.Remove(inventoryitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}