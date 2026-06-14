import os

os.makedirs("Views/Reports", exist_ok=True)

# 1. Index View
index_content = """@{ ViewData["Title"] = "ERP Raporları"; }
<div class="row mb-4">
    <div class="col">
        <h2 class="fw-bold"><i class="bi bi-bar-chart-line-fill text-primary"></i> ERP Raporlama Merkezi</h2>
        <p class="text-muted">Kurumsal analiz, gelir, gider ve personel raporlarına buradan ulaşabilirsiniz.</p>
    </div>
</div>

<div class="row g-4">
    <div class="col-md-6 col-lg-4">
        <a asp-action="MonthlyRevenue" class="text-decoration-none">
            <div class="card h-100 border-0 shadow-sm module-card border-start border-4 border-success">
                <div class="card-body">
                    <h5 class="fw-bold text-dark"><i class="bi bi-cash-stack text-success"></i> Aylık Gelir Raporu</h5>
                    <p class="text-muted small mb-0">Tüm ödeme kalemleri ve gelir analizi.</p>
                </div>
            </div>
        </a>
    </div>
    <div class="col-md-6 col-lg-4">
        <a asp-action="Expense" class="text-decoration-none">
            <div class="card h-100 border-0 shadow-sm module-card border-start border-4 border-danger">
                <div class="card-body">
                    <h5 class="fw-bold text-dark"><i class="bi bi-graph-down-arrow text-danger"></i> Gider Raporu</h5>
                    <p class="text-muted small mb-0">Operasyonel giderler ve harcamalar.</p>
                </div>
            </div>
        </a>
    </div>
    <div class="col-md-6 col-lg-4">
        <a asp-action="Employee" class="text-decoration-none">
            <div class="card h-100 border-0 shadow-sm module-card border-start border-4 border-info">
                <div class="card-body">
                    <h5 class="fw-bold text-dark"><i class="bi bi-people text-info"></i> Personel Raporu</h5>
                    <p class="text-muted small mb-0">Departman ve maaş dağılımları.</p>
                </div>
            </div>
        </a>
    </div>
    <div class="col-md-6 col-lg-4">
        <a asp-action="Inventory" class="text-decoration-none">
            <div class="card h-100 border-0 shadow-sm module-card border-start border-4 border-secondary">
                <div class="card-body">
                    <h5 class="fw-bold text-dark"><i class="bi bi-box-seam text-secondary"></i> Envanter Raporu</h5>
                    <p class="text-muted small mb-0">Otel bazlı stok durumu ve değer analizi.</p>
                </div>
            </div>
        </a>
    </div>
    <div class="col-md-6 col-lg-4">
        <a asp-action="Reservation" class="text-decoration-none">
            <div class="card h-100 border-0 shadow-sm module-card border-start border-4 border-warning">
                <div class="card-body">
                    <h5 class="fw-bold text-dark"><i class="bi bi-calendar-check text-warning"></i> Rezervasyon Raporu</h5>
                    <p class="text-muted small mb-0">Doluluk oranları ve rezervasyon listesi.</p>
                </div>
            </div>
        </a>
    </div>
</div>
"""
with open("Views/Reports/Index.cshtml", "w") as f:
    f.write(index_content)


# 2. MonthlyRevenue View
revenue_content = """@model IEnumerable<HotelERP.Models.Payment>
@{ ViewData["Title"] = "Aylık Gelir Raporu"; }
<h3 class="fw-bold mb-4">Aylık Gelir Raporu</h3>
<div class="card shadow-sm border-0">
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>Tarih</th>
                        <th>Otel</th>
                        <th>Oda Numarası</th>
                        <th>Yöntem</th>
                        <th>Tutar</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model) {
                        <tr>
                            <td>@item.PaymentDate.ToString("dd.MM.yyyy")</td>
                            <td>@item.Reservation.Room.Hotel.Name</td>
                            <td>@item.Reservation.Room.RoomNumber</td>
                            <td>@item.PaymentMethod</td>
                            <td class="text-success fw-bold">₺@item.Amount.ToString("N2")</td>
                        </tr>
                    }
                </tbody>
                <tfoot class="bg-light fw-bold">
                    <tr>
                        <td colspan="4" class="text-end">Toplam Gelir:</td>
                        <td class="text-success">₺@Model.Sum(x => x.Amount).ToString("N2")</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
"""
with open("Views/Reports/MonthlyRevenue.cshtml", "w") as f:
    f.write(revenue_content)

# 3. Expense View
expense_content = """@model IEnumerable<HotelERP.Models.Expense>
@{ ViewData["Title"] = "Gider Raporu"; }
<h3 class="fw-bold mb-4">Gider Raporu</h3>
<div class="card shadow-sm border-0">
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>Tarih</th>
                        <th>Otel</th>
                        <th>Kategori</th>
                        <th>Açıklama</th>
                        <th>Tutar</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model) {
                        <tr>
                            <td>@item.ExpenseDate.ToString("dd.MM.yyyy")</td>
                            <td>@item.Hotel.Name</td>
                            <td>@item.Category</td>
                            <td>@item.Description</td>
                            <td class="text-danger fw-bold">₺@item.Amount.ToString("N2")</td>
                        </tr>
                    }
                </tbody>
                <tfoot class="bg-light fw-bold">
                    <tr>
                        <td colspan="4" class="text-end">Toplam Gider:</td>
                        <td class="text-danger">₺@Model.Sum(x => x.Amount).ToString("N2")</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
"""
with open("Views/Reports/Expense.cshtml", "w") as f:
    f.write(expense_content)

# 4. Employee View
employee_content = """@model IEnumerable<HotelERP.Models.Employee>
@{ ViewData["Title"] = "Personel Raporu"; }
<h3 class="fw-bold mb-4">Personel Raporu</h3>
<div class="card shadow-sm border-0">
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>Ad Soyad</th>
                        <th>Otel</th>
                        <th>Departman</th>
                        <th>Pozisyon</th>
                        <th>Maaş</th>
                        <th>İşe Giriş</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model) {
                        <tr>
                            <td>@item.FullName</td>
                            <td>@item.Hotel.Name</td>
                            <td>@item.Department.Name</td>
                            <td>@item.Position</td>
                            <td>₺@item.Salary.ToString("N2")</td>
                            <td>@item.HireDate.ToString("dd.MM.yyyy")</td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    </div>
</div>
"""
with open("Views/Reports/Employee.cshtml", "w") as f:
    f.write(employee_content)

# 5. Inventory View
inventory_content = """@model IEnumerable<HotelERP.Models.InventoryItem>
@{ ViewData["Title"] = "Envanter Raporu"; }
<h3 class="fw-bold mb-4">Envanter Raporu</h3>
<div class="card shadow-sm border-0">
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>SKU</th>
                        <th>Ürün Adı</th>
                        <th>Otel</th>
                        <th>Stok Adedi</th>
                        <th>Birim Fiyat</th>
                        <th>Toplam Değer</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model) {
                        var totalValue = item.QuantityInStock * item.UnitPrice;
                        <tr>
                            <td>@item.SKU</td>
                            <td>@item.Name</td>
                            <td>@item.Hotel.Name</td>
                            <td>@item.QuantityInStock</td>
                            <td>₺@item.UnitPrice.ToString("N2")</td>
                            <td class="fw-bold">₺@totalValue.ToString("N2")</td>
                        </tr>
                    }
                </tbody>
                <tfoot class="bg-light fw-bold">
                    <tr>
                        <td colspan="5" class="text-end">Toplam Envanter Değeri:</td>
                        <td>₺@Model.Sum(x => x.QuantityInStock * x.UnitPrice).ToString("N2")</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
"""
with open("Views/Reports/Inventory.cshtml", "w") as f:
    f.write(inventory_content)

# 6. Reservation View
reservation_content = """@model IEnumerable<HotelERP.Models.Reservation>
@{ ViewData["Title"] = "Rezervasyon Raporu"; }
<h3 class="fw-bold mb-4">Rezervasyon Raporu</h3>
<div class="card shadow-sm border-0">
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>Müşteri</th>
                        <th>Otel</th>
                        <th>Oda Numarası</th>
                        <th>Giriş - Çıkış</th>
                        <th>Toplam Tutar</th>
                        <th>Durum</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model) {
                        <tr>
                            <td>@item.Customer.FullName</td>
                            <td>@item.Room.Hotel.Name</td>
                            <td>@item.Room.RoomNumber</td>
                            <td>@item.CheckInDate.ToString("dd.MM.yyyy") - @item.CheckOutDate.ToString("dd.MM.yyyy")</td>
                            <td>₺@item.TotalPrice.ToString("N2")</td>
                            <td>
                                @if (item.Status == HotelERP.Models.ReservationStatus.Confirmed) { <span class="badge bg-success">Onaylandı</span> }
                                else if (item.Status == HotelERP.Models.ReservationStatus.Pending) { <span class="badge bg-warning text-dark">Bekliyor</span> }
                                else { <span class="badge bg-secondary">@item.Status</span> }
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    </div>
</div>
"""
with open("Views/Reports/Reservation.cshtml", "w") as f:
    f.write(reservation_content)
print("Views created successfully!")
