using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelERP.Data;
using HotelERP.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HotelERP.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MonthlyRevenue()
        {
            var revenues = await _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Room)
                .ThenInclude(r => r.Hotel)
                .Where(p => p.Status == PaymentStatus.Paid)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return View(revenues);
        }

        public async Task<IActionResult> Expense()
        {
            var expenses = await _context.Expenses
                .Include(e => e.Hotel)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return View(expenses);
        }

        public async Task<IActionResult> Employee()
        {
            var employees = await _context.Employees
                .Include(e => e.Hotel)
                .Include(e => e.Department)
                .OrderBy(e => e.Hotel.Name).ThenBy(e => e.Department.Name)
                .ToListAsync();

            return View(employees);
        }

        public async Task<IActionResult> Inventory()
        {
            var items = await _context.InventoryItems
                .Include(i => i.Hotel)
                .OrderBy(i => i.Hotel.Name).ThenBy(i => i.Name)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Reservation()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .ThenInclude(r => r.Hotel)
                .Include(r => r.Customer)
                .OrderByDescending(r => r.CheckInDate)
                .ToListAsync();

            return View(reservations);
        }
    }
}
