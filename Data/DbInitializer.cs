using HotelERP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelERP.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.Hotels.Any()) return; // DB has been seeded

            var rnd = new Random(12345); // Fixed seed for reproducibility

            // 1. Hotels (5)
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "Grand Sapphire Hotel", Address = "Antalya, Turkey", Phone = "0242 111 2233", Email = "info@grandsapphire.com", Stars = 5, Description = "Lüks sahil oteli." },
                new Hotel { Name = "City Plaza Executive", Address = "Istanbul, Turkey", Phone = "0212 333 4455", Email = "contact@cityplaza.com", Stars = 4, Description = "İş merkezlerine yakın." },
                new Hotel { Name = "Aegean Pearl Resort", Address = "Izmir, Turkey", Phone = "0232 555 6677", Email = "hello@aegeanpearl.com", Stars = 5, Description = "Ege'nin incisi." },
                new Hotel { Name = "Mountain View Lodge", Address = "Bursa, Turkey", Phone = "0224 777 8899", Email = "stay@mountainview.com", Stars = 3, Description = "Doğa ile iç içe." },
                new Hotel { Name = "Cappadocia Cave Suites", Address = "Nevsehir, Turkey", Phone = "0384 999 0011", Email = "booking@cavesuites.com", Stars = 4, Description = "Tarihi mağara odaları." }
            };
            context.Hotels.AddRange(hotels);
            context.SaveChanges();

            // 2. Departments (6 per hotel)
            var deptNames = new[] { "Reception", "HR", "Accounting", "Purchasing", "Housekeeping", "Management" };
            var departments = new List<Department>();
            foreach (var h in hotels)
            {
                foreach (var dn in deptNames)
                {
                    departments.Add(new Department { Name = dn, Description = $"{dn} operations", HotelId = h.Id });
                }
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();

            // 3. Employees (20)
            var firstNames = new[] { "Ahmet", "Mehmet", "Ayşe", "Fatma", "Ali", "Can", "Zeynep", "Elif", "Burak", "Emre" };
            var lastNames = new[] { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Öztürk", "Aydın", "Özdemir", "Arslan" };
            var employees = new List<Employee>();
            for (int i = 0; i < 20; i++)
            {
                var h = hotels[rnd.Next(hotels.Count)];
                var hDepts = departments.Where(d => d.HotelId == h.Id).ToList();
                employees.Add(new Employee
                {
                    FirstName = firstNames[rnd.Next(firstNames.Length)],
                    LastName = lastNames[rnd.Next(lastNames.Length)],
                    Position = (EmployeePosition)rnd.Next(0, 5),
                    Salary = rnd.Next(25000, 75000),
                    HireDate = DateTime.Today.AddDays(-rnd.Next(100, 2000)),
                    HotelId = h.Id,
                    DepartmentId = hDepts[rnd.Next(hDepts.Count)].Id,
                    Email = $"emp{i}@hotel.com",
                    Phone = $"555{rnd.Next(1000000, 9999999)}"
                });
            }
            context.Employees.AddRange(employees);
            context.SaveChanges();

            // 4. InventoryItems (8 types per hotel)
            var invNames = new[] { "Towels", "Shampoo", "Soap", "Bedsheets", "Cleaning Chemicals", "Water Bottles", "Coffee Supplies", "Restaurant Supplies" };
            var inventoryItems = new List<InventoryItem>();
            foreach (var h in hotels)
            {
                for (int i = 0; i < invNames.Length; i++)
                {
                    inventoryItems.Add(new InventoryItem
                    {
                        Name = invNames[i],
                        SKU = $"INV-{h.Id}-{i}",
                        QuantityInStock = rnd.Next(50, 1000),
                        UnitPrice = Convert.ToDecimal(rnd.Next(10, 500)),
                        HotelId = h.Id
                    });
                }
            }
            context.InventoryItems.AddRange(inventoryItems);
            context.SaveChanges();

            // 5. Suppliers (10)
            var suppliers = new List<Supplier>();
            for (int i = 1; i <= 10; i++)
            {
                suppliers.Add(new Supplier
                {
                    CompanyName = $"Supplier Global {i} A.Ş.",
                    ContactPerson = $"Contact {i}",
                    Email = $"supplier{i}@global.com",
                    Phone = $"55511122{i:00}",
                    Address = $"Business District {i}"
                });
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            // 6. PurchaseOrders (20)
            var purchaseOrders = new List<PurchaseOrder>();
            for (int i = 0; i < 20; i++)
            {
                var h = hotels[rnd.Next(hotels.Count)];
                var hInvs = inventoryItems.Where(inv => inv.HotelId == h.Id).ToList();
                var inv = hInvs[rnd.Next(hInvs.Count)];
                int qty = rnd.Next(10, 200);

                purchaseOrders.Add(new PurchaseOrder
                {
                    SupplierId = suppliers[rnd.Next(suppliers.Count)].Id,
                    HotelId = h.Id,
                    InventoryItemId = inv.Id,
                    Quantity = qty,
                    OrderDate = DateTime.Today.AddDays(-rnd.Next(1, 100)),
                    TotalAmount = inv.UnitPrice * qty,
                    Status = (PurchaseOrderStatus)rnd.Next(0, 4)
                });
            }
            context.PurchaseOrders.AddRange(purchaseOrders);
            context.SaveChanges();

            // 7. Rooms (50 across hotels)
            var rooms = new List<Room>();
            for (int i = 1; i <= 50; i++)
            {
                var h = hotels[i % hotels.Count];
                var rType = (RoomType)rnd.Next(0, 4);
                decimal price = rType == RoomType.Single ? 1500 : rType == RoomType.Double ? 2500 : rType == RoomType.Suite ? 5000 : 7500;
                rooms.Add(new Room
                {
                    RoomNumber = $"{i}01",
                    RoomType = rType,
                    PricePerNight = price,
                    IsAvailable = true,
                    HotelId = h.Id
                });
            }
            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            // 8. Customers (30)
            var customers = new List<Customer>();
            for (int i = 0; i < 30; i++)
            {
                customers.Add(new Customer
                {
                    FirstName = firstNames[rnd.Next(firstNames.Length)],
                    LastName = lastNames[rnd.Next(lastNames.Length)],
                    Email = $"customer{i}@example.com",
                    Phone = $"532{rnd.Next(1000000, 9999999)}",
                    NationalId = $"{rnd.Next(100000000, 999999999)}00",
                    Address = "Turkey"
                });
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();

            // 9. Reservations (40)
            var reservations = new List<Reservation>();
            for (int i = 0; i < 40; i++)
            {
                var r = rooms[rnd.Next(rooms.Count)];
                var c = customers[rnd.Next(customers.Count)];
                int days = rnd.Next(1, 10);
                
                // Some past, some future
                DateTime checkIn = DateTime.Today.AddDays(rnd.Next(-30, 30));
                DateTime checkOut = checkIn.AddDays(days);
                
                var res = new Reservation
                {
                    RoomId = r.Id,
                    CustomerId = c.Id,
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    TotalPrice = r.PricePerNight * days,
                    Status = checkIn > DateTime.Today ? ReservationStatus.Pending : ReservationStatus.Completed
                };

                // Update room availability based on status
                if (checkIn <= DateTime.Today && checkOut >= DateTime.Today && res.Status != ReservationStatus.Cancelled)
                {
                    res.Status = ReservationStatus.Confirmed;
                    r.IsAvailable = false;
                }
                
                reservations.Add(res);
            }
            context.Reservations.AddRange(reservations);
            context.SaveChanges();

            // 10. Payments (50)
            var payments = new List<Payment>();
            for (int i = 0; i < 50; i++)
            {
                var res = reservations[rnd.Next(reservations.Count)];
                payments.Add(new Payment
                {
                    ReservationId = res.Id,
                    Amount = res.TotalPrice / rnd.Next(1, 3), // Partial or full payment
                    PaymentDate = res.CheckInDate.AddDays(rnd.Next(0, 2)),
                    PaymentMethod = (PaymentMethod)rnd.Next(0, 3),
                    Status = PaymentStatus.Paid
                });
            }
            context.Payments.AddRange(payments);
            context.SaveChanges();

            // 11. Expenses (30)
            var expenseDescs = new[] { "Electric Bill", "Water Bill", "Internet", "Maintenance Repair", "Marketing Campaign", "Staff Bonus" };
            var expenses = new List<Expense>();
            for (int i = 0; i < 30; i++)
            {
                var h = hotels[rnd.Next(hotels.Count)];
                expenses.Add(new Expense
                {
                    HotelId = h.Id,
                    Description = expenseDescs[rnd.Next(expenseDescs.Length)],
                    Amount = Convert.ToDecimal(rnd.Next(1000, 50000)),
                    ExpenseDate = DateTime.Today.AddDays(-rnd.Next(1, 60)),
                    Category = (ExpenseCategory)rnd.Next(0, 5)
                });
            }
            context.Expenses.AddRange(expenses);
            context.SaveChanges();
            // 12. StockMovements (40)
            var stockMovements = new List<StockMovement>();
            for (int i = 0; i < 40; i++)
            {
                var inv = inventoryItems[rnd.Next(inventoryItems.Count)];
                int qty = rnd.Next(-50, 200); // negative for out, positive for in
                if (qty == 0) qty = 10;
                stockMovements.Add(new StockMovement
                {
                    InventoryItemId = inv.Id,
                    Quantity = qty,
                    MovementDate = DateTime.Today.AddDays(-rnd.Next(1, 60)),
                    Description = qty > 0 ? "Tedarikçi Girişi" : "Departman Kullanımı"
                });
            }
            context.StockMovements.AddRange(stockMovements);
            context.SaveChanges();

            // 13. CustomerRequests (20)
            var requests = new[] { "Ekstra yastık", "Geç çıkış", "Oda temizliği", "Şampuan", "Uyandırma servisi", "Havaalanı transferi", "Bebek yatağı", "Vegan kahvaltı" };
            var customerRequests = new List<CustomerRequest>();
            for (int i = 0; i < 20; i++)
            {
                var res = reservations[rnd.Next(reservations.Count)];
                customerRequests.Add(new CustomerRequest
                {
                    CustomerId = res.CustomerId,
                    ReservationId = res.Id,
                    RequestText = requests[rnd.Next(requests.Length)],
                    RequestDate = res.CheckInDate.AddDays(rnd.Next(0, 2)),
                    IsResolved = rnd.Next(0, 2) == 1
                });
            }
            context.CustomerRequests.AddRange(customerRequests);
            context.SaveChanges();

            // 14. CustomerFeedbacks (25)
            var comments = new[] { "Harika bir deneyimdi.", "Oda temizliği daha iyi olabilirdi.", "Yemekler çok lezzetliydi.", "Personel çok ilgiliydi.", "Fiyat/performans açısından başarılı.", "Gürültülü bir odaydı.", "Tekrar geleceğiz." };
            var customerFeedbacks = new List<CustomerFeedback>();
            for (int i = 0; i < 25; i++)
            {
                var res = reservations.Where(r => r.Status == ReservationStatus.Completed).ToList();
                if (res.Any())
                {
                    var r = res[rnd.Next(res.Count)];
                    customerFeedbacks.Add(new CustomerFeedback
                    {
                        CustomerId = r.CustomerId,
                        ReservationId = r.Id,
                        Rating = rnd.Next(2, 6),
                        Comment = comments[rnd.Next(comments.Length)],
                        FeedbackDate = r.CheckOutDate.AddDays(rnd.Next(1, 5))
                    });
                }
            }
            context.CustomerFeedbacks.AddRange(customerFeedbacks);
            context.SaveChanges();
        }
    }
}
