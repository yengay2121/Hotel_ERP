using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelERP.Data;
using HotelERP.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace HotelERP.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Room)
                    .ThenInclude(ro => ro!.Hotel);
            return View(await reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Room)
                    .ThenInclude(ro => ro!.Hotel)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,CustomerId,CheckInDate,CheckOutDate,Status")] Reservation reservation)
        {
            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room == null)
            {
                ModelState.AddModelError("RoomId", "Geçersiz oda seçimi.");
            }

            if (reservation.CheckOutDate <= reservation.CheckInDate)
            {
                ModelState.AddModelError("CheckOutDate", "Çıkış tarihi giriş tarihinden sonra olmalıdır.");
            }

            if (ModelState.IsValid && room != null)
            {
                // Calculate Price
                var days = (reservation.CheckOutDate - reservation.CheckInDate).Days;
                if (days <= 0) days = 1;
                reservation.TotalPrice = days * room.PricePerNight;

                // ERP logic: Update Room Availability
                if (reservation.Status == ReservationStatus.Confirmed)
                {
                    room.IsAvailable = false;
                    _context.Update(room);
                }

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists(reservation.RoomId, reservation.CustomerId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            PopulateDropDownLists(reservation.RoomId, reservation.CustomerId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,CustomerId,CheckInDate,CheckOutDate,Status")] Reservation reservation)
        {
            if (id != reservation.Id) return NotFound();

            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room == null)
            {
                ModelState.AddModelError("RoomId", "Geçersiz oda seçimi.");
            }

            if (reservation.CheckOutDate <= reservation.CheckInDate)
            {
                ModelState.AddModelError("CheckOutDate", "Çıkış tarihi giriş tarihinden sonra olmalıdır.");
            }

            if (ModelState.IsValid && room != null)
            {
                try
                {
                    // Track previous room availability state
                    var oldReservation = await _context.Reservations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                    if (oldReservation != null)
                    {
                        // Release old room if different or if status changed
                        if (oldReservation.RoomId != reservation.RoomId || oldReservation.Status != reservation.Status)
                        {
                            var oldRoom = await _context.Rooms.FindAsync(oldReservation.RoomId);
                            if (oldRoom != null)
                            {
                                oldRoom.IsAvailable = true;
                                _context.Update(oldRoom);
                            }
                        }
                    }

                    // Recalculate price
                    var days = (reservation.CheckOutDate - reservation.CheckInDate).Days;
                    if (days <= 0) days = 1;
                    reservation.TotalPrice = days * room.PricePerNight;

                    // Update current room availability based on status
                    if (reservation.Status == ReservationStatus.Confirmed)
                    {
                        room.IsAvailable = false;
                    }
                    else
                    {
                        room.IsAvailable = true;
                    }
                    _context.Update(room);

                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists(reservation.RoomId, reservation.CustomerId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Room)
                    .ThenInclude(ro => ro!.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null) return NotFound();

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                // Release room
                var room = await _context.Rooms.FindAsync(reservation.RoomId);
                if (room != null)
                {
                    room.IsAvailable = true;
                    _context.Update(room);
                }

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDownLists(object? selectedRoom = null, object? selectedCustomer = null)
        {
            var roomQuery = _context.Rooms.Include(r => r.Hotel)
                .Select(r => new
                {
                    Id = r.Id,
                    Description = $"{r.Hotel!.Name} - Oda {r.RoomNumber} [{r.RoomType}] ({r.PricePerNight} TL)"
                });

            var customerQuery = _context.Customers
                .Select(c => new
                {
                    Id = c.Id,
                    FullName = $"{c.FirstName} {c.LastName} - TC: {c.NationalId}"
                });

            ViewData["RoomId"] = new SelectList(roomQuery, "Id", "Description", selectedRoom);
            ViewData["CustomerId"] = new SelectList(customerQuery, "Id", "FullName", selectedCustomer);
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
