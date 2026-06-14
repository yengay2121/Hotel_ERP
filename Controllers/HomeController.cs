using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelERP.Models;
using HotelERP.Data;
using System.Linq;

namespace HotelERP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var now = DateTime.Today;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        
        // 1. Active Reservations
        ViewBag.ActiveReservations = _context.Reservations
            .Count(r => r.Status == ReservationStatus.Confirmed && r.CheckInDate <= now && r.CheckOutDate >= now);

        // 2. Occupied Rooms
        ViewBag.OccupiedRooms = _context.Rooms.Count(r => !r.IsAvailable);

        // 3. Available Rooms
        ViewBag.AvailableRooms = _context.Rooms.Count(r => r.IsAvailable);

        // 4. Monthly Revenue
        ViewBag.MonthlyRevenue = _context.Payments
            .Where(p => p.Status == PaymentStatus.Paid && p.PaymentDate >= startOfMonth)
            .Sum(p => p.Amount);

        // 5. Monthly Expenses
        ViewBag.MonthlyExpenses = _context.Expenses
            .Where(e => e.ExpenseDate >= startOfMonth)
            .Sum(e => e.Amount);

        // 6. Employee Count
        ViewBag.EmployeeCount = _context.Employees.Count();

        // 7. Inventory Value
        ViewBag.InventoryValue = _context.InventoryItems
            .Sum(i => i.QuantityInStock * i.UnitPrice);

        // 8. Purchase Orders This Month
        ViewBag.MonthlyPurchaseOrders = _context.PurchaseOrders
            .Count(po => po.OrderDate >= startOfMonth);

        // 9. Customer Count
        ViewBag.CustomerCount = _context.Customers.Count();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
