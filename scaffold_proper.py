import os

models = {
    "Department": {
        "plural": "Departments",
        "has_hotel": True,
        "props": ["Name", "Description", "HotelId"]
    },
    "InventoryItem": {
        "plural": "InventoryItems",
        "has_hotel": True,
        "props": ["Name", "SKU", "QuantityInStock", "UnitPrice", "HotelId"]
    },
    "Supplier": {
        "plural": "Suppliers",
        "has_hotel": False,
        "props": ["CompanyName", "ContactPerson", "Email", "Phone", "Address"]
    },
    "PurchaseOrder": {
        "plural": "PurchaseOrders",
        "has_hotel": True,
        "has_supplier": True,
        "props": ["SupplierId", "HotelId", "OrderDate", "TotalAmount", "Status"]
    },
    "Expense": {
        "plural": "Expenses",
        "has_hotel": True,
        "props": ["HotelId", "Description", "Amount", "ExpenseDate", "Category"]
    }
}

def generate_controller(model, info):
    plural = info["plural"]
    props_str = ",".join(["Id"] + info["props"])
    
    includes = ""
    include_str = ""
    if info.get("has_hotel") and info.get("has_supplier"):
        includes = ".Include(m => m.Hotel).Include(m => m.Supplier)"
        include_str = ".Include(m => m.Hotel).Include(m => m.Supplier)"
    elif info.get("has_hotel"):
        includes = ".Include(m => m.Hotel)"
        include_str = ".Include(m => m.Hotel)"
        
    select_lists = ""
    viewdata_create = ""
    if info.get("has_hotel"):
        select_lists += f'            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", {model.lower()}?.HotelId);\n'
        viewdata_create += f'            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");\n'
    if info.get("has_supplier"):
        select_lists += f'            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CompanyName", {model.lower()}?.SupplierId);\n'
        viewdata_create += f'            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CompanyName");\n'

    code = f"""using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelERP.Data;
using HotelERP.Models;

namespace HotelERP.Controllers
{{
    public class {plural}Controller : Controller
    {{
        private readonly ApplicationDbContext _context;

        public {plural}Controller(ApplicationDbContext context)
        {{
            _context = context;
        }}

        public async Task<IActionResult> Index()
        {{
            var appDbContext = _context.{plural}{include_str};
            return View(await appDbContext.ToListAsync());
        }}

        public async Task<IActionResult> Details(int? id)
        {{
            if (id == null) return NotFound();
            var {model.lower()} = await _context.{plural}{include_str}.FirstOrDefaultAsync(m => m.Id == id);
            if ({model.lower()} == null) return NotFound();
            return View({model.lower()});
        }}

        public IActionResult Create()
        {{
{viewdata_create}            return View();
        }}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("{props_str}")] {model} {model.lower()})
        {{
            if (ModelState.IsValid)
            {{
                _context.Add({model.lower()});
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }}
{select_lists}            return View({model.lower()});
        }}

        public async Task<IActionResult> Edit(int? id)
        {{
            if (id == null) return NotFound();
            var {model.lower()} = await _context.{plural}.FindAsync(id);
            if ({model.lower()} == null) return NotFound();
{select_lists}            return View({model.lower()});
        }}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("{props_str}")] {model} {model.lower()})
        {{
            if (id != {model.lower()}.Id) return NotFound();
            if (ModelState.IsValid)
            {{
                try
                {{
                    _context.Update({model.lower()});
                    await _context.SaveChangesAsync();
                }}
                catch (DbUpdateConcurrencyException)
                {{
                    if (!_context.{plural}.Any(e => e.Id == {model.lower()}.Id)) return NotFound();
                    else throw;
                }}
                return RedirectToAction(nameof(Index));
            }}
{select_lists}            return View({model.lower()});
        }}

        public async Task<IActionResult> Delete(int? id)
        {{
            if (id == null) return NotFound();
            var {model.lower()} = await _context.{plural}{include_str}.FirstOrDefaultAsync(m => m.Id == id);
            if ({model.lower()} == null) return NotFound();
            return View({model.lower()});
        }}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {{
            var {model.lower()} = await _context.{plural}.FindAsync(id);
            if ({model.lower()} != null) _context.{plural}.Remove({model.lower()});
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }}
    }}
}}"""
    os.makedirs("Controllers", exist_ok=True)
    with open(f"Controllers/{plural}Controller.cs", "w") as f:
        f.write(code)

def generate_views(model, info):
    plural = info["plural"]
    os.makedirs(f"Views/{plural}", exist_ok=True)
    
    th_tags = "\\n".join([f"            <th>@Html.DisplayNameFor(model => model.{p})</th>" for p in info["props"]])
    td_tags = ""
    for p in info["props"]:
        if p == "HotelId":
            td_tags += f"            <td>@Html.DisplayFor(modelItem => item.Hotel.Name)</td>\\n"
        elif p == "SupplierId":
            td_tags += f"            <td>@Html.DisplayFor(modelItem => item.Supplier.CompanyName)</td>\\n"
        else:
            td_tags += f"            <td>@Html.DisplayFor(modelItem => item.{p})</td>\\n"
            
    index_view = f"""@model IEnumerable<HotelERP.Models.{model}>
@{{ ViewData["Title"] = "Index - {plural}"; }}
<div class="card shadow-sm border-0 mb-4">
    <div class="card-header bg-white d-flex justify-content-between align-items-center py-3">
        <h4 class="mb-0 fw-bold"><i class="bi bi-table text-primary"></i> {plural}</h4>
        <a asp-action="Create" class="btn btn-primary"><i class="bi bi-plus-lg"></i> Yeni Oluştur</a>
    </div>
    <div class="card-body p-0">
        <div class="table-responsive">
            <table class="table table-hover align-middle mb-0">
                <thead class="bg-light">
                    <tr>
{th_tags}
                        <th class="text-end">İşlemler</th>
                    </tr>
                </thead>
                <tbody>
@foreach (var item in Model) {{
                    <tr>
{td_tags}                        <td class="text-end">
                            <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm btn-outline-primary"><i class="bi bi-pencil"></i></a>
                            <a asp-action="Details" asp-route-id="@item.Id" class="btn btn-sm btn-outline-info"><i class="bi bi-eye"></i></a>
                            <a asp-action="Delete" asp-route-id="@item.Id" class="btn btn-sm btn-outline-danger"><i class="bi bi-trash"></i></a>
                        </td>
                    </tr>
}}
                </tbody>
            </table>
        </div>
    </div>
</div>"""

    with open(f"Views/{plural}/Index.cshtml", "w") as f:
        f.write(index_view)

    inputs = ""
    for p in info["props"]:
        if p == "HotelId":
            inputs += f"""            <div class="form-group mb-3">
                <label asp-for="{p}" class="control-label"></label>
                <select asp-for="{p}" class="form-control" asp-items="ViewBag.HotelId"></select>
            </div>\n"""
        elif p == "SupplierId":
            inputs += f"""            <div class="form-group mb-3">
                <label asp-for="{p}" class="control-label"></label>
                <select asp-for="{p}" class="form-control" asp-items="ViewBag.SupplierId"></select>
            </div>\n"""
        else:
            inputs += f"""            <div class="form-group mb-3">
                <label asp-for="{p}" class="control-label"></label>
                <input asp-for="{p}" class="form-control" />
                <span asp-validation-for="{p}" class="text-danger"></span>
            </div>\n"""

    create_view = f"""@model HotelERP.Models.{model}
@{{ ViewData["Title"] = "Create - {model}"; }}
<div class="row">
    <div class="col-md-6 mx-auto">
        <div class="card shadow-sm border-0">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold">Yeni {model}</h5>
            </div>
            <div class="card-body">
                <form asp-action="Create">
                    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
{inputs}
                    <div class="form-group mt-4">
                        <input type="submit" value="Kaydet" class="btn btn-primary px-4" />
                        <a asp-action="Index" class="btn btn-light ms-2">Listeye Dön</a>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>"""

    with open(f"Views/{plural}/Create.cshtml", "w") as f:
        f.write(create_view)

    edit_view = f"""@model HotelERP.Models.{model}
@{{ ViewData["Title"] = "Edit - {model}"; }}
<div class="row">
    <div class="col-md-6 mx-auto">
        <div class="card shadow-sm border-0">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold">{model} Düzenle</h5>
            </div>
            <div class="card-body">
                <form asp-action="Edit">
                    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                    <input type="hidden" asp-for="Id" />
{inputs}
                    <div class="form-group mt-4">
                        <input type="submit" value="Güncelle" class="btn btn-primary px-4" />
                        <a asp-action="Index" class="btn btn-light ms-2">Listeye Dön</a>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>"""

    with open(f"Views/{plural}/Edit.cshtml", "w") as f:
        f.write(edit_view)

    dt_dd = ""
    for p in info["props"]:
        if p == "HotelId":
            dt_dd += f"        <dt class=\"col-sm-4\">Hotel</dt>\\n        <dd class=\"col-sm-8\">@Html.DisplayFor(model => model.Hotel.Name)</dd>\\n"
        elif p == "SupplierId":
            dt_dd += f"        <dt class=\"col-sm-4\">Supplier</dt>\\n        <dd class=\"col-sm-8\">@Html.DisplayFor(model => model.Supplier.CompanyName)</dd>\\n"
        else:
            dt_dd += f"        <dt class=\"col-sm-4\">@Html.DisplayNameFor(model => model.{p})</dt>\\n        <dd class=\"col-sm-8\">@Html.DisplayFor(model => model.{p})</dd>\\n"

    details_view = f"""@model HotelERP.Models.{model}
@{{ ViewData["Title"] = "Details - {model}"; }}
<div class="row">
    <div class="col-md-8 mx-auto">
        <div class="card shadow-sm border-0">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold">{model} Detayları</h5>
            </div>
            <div class="card-body">
                <dl class="row mb-0">
{dt_dd}                </dl>
            </div>
            <div class="card-footer bg-white py-3">
                <a asp-action="Edit" asp-route-id="@Model?.Id" class="btn btn-primary">Düzenle</a>
                <a asp-action="Index" class="btn btn-light ms-2">Listeye Dön</a>
            </div>
        </div>
    </div>
</div>"""

    with open(f"Views/{plural}/Details.cshtml", "w") as f:
        f.write(details_view)
        
    delete_view = f"""@model HotelERP.Models.{model}
@{{ ViewData["Title"] = "Delete - {model}"; }}
<div class="row">
    <div class="col-md-8 mx-auto">
        <div class="alert alert-danger shadow-sm border-0">
            <h5 class="fw-bold"><i class="bi bi-exclamation-triangle-fill"></i> Silme Onayı</h5>
            <p>Bu kaydı silmek istediğinize emin misiniz? Bu işlem geri alınamaz.</p>
        </div>
        <div class="card shadow-sm border-0">
            <div class="card-body">
                <dl class="row mb-0">
{dt_dd}                </dl>
            </div>
            <div class="card-footer bg-white py-3">
                <form asp-action="Delete">
                    <input type="hidden" asp-for="Id" />
                    <input type="submit" value="Sil" class="btn btn-danger px-4" />
                    <a asp-action="Index" class="btn btn-light ms-2">İptal</a>
                </form>
            </div>
        </div>
    </div>
</div>"""

    with open(f"Views/{plural}/Delete.cshtml", "w") as f:
        f.write(delete_view)

for m, info in models.items():
    generate_controller(m, info)
    generate_views(m, info)

print("Scaffolding complete.")
