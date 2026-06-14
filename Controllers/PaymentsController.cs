using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelERP.Data;
using HotelERP.Models;
using System.Threading.Tasks;
using System.Linq;

namespace HotelERP.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r!.Customer)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r!.Room)
                        .ThenInclude(ro => ro!.Hotel);
            return View(await payments.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r!.Customer)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r!.Room)
                        .ThenInclude(ro => ro!.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            PopulateReservationsDropDownList();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationId,Amount,PaymentDate,PaymentMethod,Status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateReservationsDropDownList(payment.ReservationId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            PopulateReservationsDropDownList(payment.ReservationId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationId,Amount,PaymentDate,PaymentMethod,Status")] Payment payment)
        {
            if (id != payment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateReservationsDropDownList(payment.ReservationId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r!.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void PopulateReservationsDropDownList(object? selectedReservation = null)
        {
            var reservationsQuery = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Room)
                    .ThenInclude(ro => ro!.Hotel)
                .Select(r => new
                {
                    Id = r.Id,
                    Description = $"Rez #{r.Id} - {r.Customer!.FirstName} {r.Customer.LastName} ({r.Room!.Hotel!.Name} Oda {r.Room.RoomNumber}) - Toplam: {r.TotalPrice} TL"
                });

            ViewData["ReservationId"] = new SelectList(reservationsQuery, "Id", "Description", selectedReservation);
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
