# Grand Otel ERP — Veritabanı ve ERP Tabloları Referans Belgesi

## ER Diyagramı (Varlık-İlişki)

```
  ┌────────────────────┐          ┌────────────────────┐
  │       Hotels       │          │     Employees      │
  ├────────────────────┤          ├────────────────────┤
  │ PK  Id (INT)       │◄────1:N──│ PK  Id (INT)       │
  │     Name           │          │     FirstName      │
  │     Address        │          │     LastName       │
  │     Phone          │          │     Position (ENUM)│
  │     Email          │          │     Salary         │
  │     Stars (1-5)    │          │     HireDate       │
  │     Description    │          │     Email          │
  └────────────────────┘          │     Phone          │
           │                      │ FK  HotelId        │
          1:N                     └────────────────────┘
           │
  ┌────────────────────┐
  │       Rooms        │
  ├────────────────────┤
  │ PK  Id (INT)       │
  │     RoomNumber     │
  │     RoomType (ENUM)│
  │     PricePerNight  │
  │     IsAvailable    │
  │ FK  HotelId        │
  └────────────────────┘
           │
          1:N
           │
  ┌────────────────────┐          ┌────────────────────┐
  │    Reservations    │          │     Customers      │
  ├────────────────────┤          ├────────────────────┤
  │ PK  Id (INT)       │──N:1────►│ PK  Id (INT)       │
  │ FK  RoomId         │          │     FirstName      │
  │ FK  CustomerId     │          │     LastName       │
  │     CheckInDate    │          │     Email          │
  │     CheckOutDate   │          │     Phone          │
  │     TotalPrice     │          │     NationalId     │
  │     Status (ENUM)  │          │     Address        │
  └────────────────────┘          └────────────────────┘
           │
          1:N
           │
  ┌────────────────────┐
  │      Payments      │
  ├────────────────────┤
  │ PK  Id (INT)       │
  │ FK  ReservationId  │
  │     Amount         │
  │     PaymentDate    │
  │     PaymentMethod  │
  │     Status (ENUM)  │
  └────────────────────┘
```

---

## İlişki Özeti

| İlişki | Tür | Kısıt (Silme) |
|---|---|---|
| Hotels → Rooms | 1'e Çok (1:N) | CASCADE (Otel silinince odalar silinir) |
| Hotels → Employees | 1'e Çok (1:N) | CASCADE (Otel silinince personel silinir) |
| Rooms → Reservations | 1'e Çok (1:N) | RESTRICT (Aktif rezervasyonu olan oda silinemez) |
| Customers → Reservations | 1'e Çok (1:N) | RESTRICT (Rezervasyonu olan müşteri silinemez) |
| Reservations → Payments | 1'e Çok (1:N) | CASCADE (Rezervasyon silinince ödemeler silinir) |

---

## 1. Hotels Tablosu — Otel Yönetimi Modülü

> **ERP Modülü:** Otel Yönetimi  
> **Açıklama:** Sisteme kayıtlı tüm otel tesislerini tutar. Diğer tüm modüller bu tabloya bağlıdır.

| Sütun | Veri Tipi | Kısıtlama | Açıklama |
|---|---|---|---|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Otelin benzersiz kimliği |
| `Name` | TEXT | NOT NULL, MAX 100 | Otel adı |
| `Address` | TEXT | NOT NULL, MAX 200 | Otel adresi |
| `Phone` | TEXT | NOT NULL | Otel telefon numarası |
| `Email` | TEXT | NOT NULL, EMAIL FORMAT | Otel e-posta adresi |
| `Stars` | INTEGER | NOT NULL, 1–5 ARASI | Yıldız sayısı (1 ile 5 arasında) |
| `Description` | TEXT | — | Otel açıklaması |

**ENUM / Sabit Değerler:** Yok  
**İlişkiler:**
- `Hotels.Id` → `Rooms.HotelId` (1:N, CASCADE)
- `Hotels.Id` → `Employees.HotelId` (1:N, CASCADE)

---

## 2. Rooms Tablosu — Oda Yönetimi Modülü

> **ERP Modülü:** Oda Yönetimi  
> **Açıklama:** Otellere ait odaları, tiplerini, gecelik ücretlerini ve müsaitlik durumlarını tutar. Rezervasyon oluşturulduğunda `IsAvailable` alanı otomatik güncellenir.

| Sütun | Veri Tipi | Kısıtlama | Açıklama |
|---|---|---|---|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Odanın benzersiz kimliği |
| `RoomNumber` | TEXT | NOT NULL | Oda numarası (Örn: "101", "203A") |
| `RoomType` | INTEGER | NOT NULL, ENUM | Oda tipi (aşağıda açıklanmıştır) |
| `PricePerNight` | REAL | NOT NULL, > 0 | Gecelik konaklama ücreti (TL) |
| `IsAvailable` | INTEGER (bool) | NOT NULL, DEFAULT 1 | Oda müsait mi? (1=Müsait, 0=Dolu) |
| `HotelId` | INTEGER | FOREIGN KEY, NOT NULL | Odanın bağlı olduğu otelin Id'si |

**RoomType ENUM Değerleri:**

| Değer | Kod | Açıklama |
|---|---|---|
| `0` | Single | Tek Kişilik Oda |
| `1` | Double | Çift Kişilik Oda |
| `2` | Suite | Süit Oda |
| `3` | Deluxe | Deluxe Oda |

**İlişkiler:**
- `Rooms.HotelId` → `Hotels.Id` (N:1)
- `Rooms.Id` → `Reservations.RoomId` (1:N, RESTRICT)

---

## 3. Customers Tablosu — Müşteri Yönetimi Modülü

> **ERP Modülü:** Müşteri Yönetimi (CRM)  
> **Açıklama:** Sisteme kayıtlı tüm müşteri profillerini saklar. Müşteri TC Kimlik veya Pasaport numarası ile tanımlanır.

| Sütun | Veri Tipi | Kısıtlama | Açıklama |
|---|---|---|---|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Müşterinin benzersiz kimliği |
| `FirstName` | TEXT | NOT NULL, MAX 50 | Müşteri adı |
| `LastName` | TEXT | NOT NULL, MAX 50 | Müşteri soyadı |
| `Email` | TEXT | NOT NULL, EMAIL FORMAT | E-posta adresi |
| `Phone` | TEXT | NOT NULL, PHONE FORMAT | Telefon numarası |
| `NationalId` | TEXT | NOT NULL, MAX 20 | TC Kimlik No veya Pasaport No |
| `Address` | TEXT | — | İkamet adresi |

**Hesaplanan Alan (C# Property):**
- `FullName` → `FirstName + " " + LastName` (veritabanında saklanmaz)

**İlişkiler:**
- `Customers.Id` → `Reservations.CustomerId` (1:N, RESTRICT)

---

## 4. Reservations Tablosu — Rezervasyon Yönetimi Modülü

> **ERP Modülü:** Rezervasyon Yönetimi  
> **Açıklama:** Müşteri-Oda eşleşmesini ve konaklama tarih aralığını tutar. `TotalPrice`, oluşturma anında sistem tarafından otomatik hesaplanır. Durum değişikliği oda müsaitliğini günceller.

| Sütun | Veri Tipi | Kısıtlama | Açıklama |
|---|---|---|---|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Rezervasyonun benzersiz kimliği |
| `RoomId` | INTEGER | FOREIGN KEY, NOT NULL | Rezerve edilen odanın Id'si |
| `CustomerId` | INTEGER | FOREIGN KEY, NOT NULL | Rezervasyonu yapan müşterinin Id'si |
| `CheckInDate` | TEXT (DATE) | NOT NULL | Giriş tarihi |
| `CheckOutDate` | TEXT (DATE) | NOT NULL, > CheckInDate | Çıkış tarihi |
| `TotalPrice` | REAL | NOT NULL, SİSTEM HESAPLAR | Toplam tutar = gün sayısı × gecelik ücret |
| `Status` | INTEGER | NOT NULL, ENUM | Rezervasyon durumu (aşağıda açıklanmıştır) |

**ReservationStatus ENUM Değerleri:**

| Değer | Kod | Açıklama | Oda Durumu Etkisi |
|---|---|---|---|
| `0` | Pending | Beklemede | Oda serbest kalır |
| `1` | Confirmed | Onaylandı | Oda **Dolu (IsAvailable=false)** yapılır |
| `2` | Cancelled | İptal Edildi | Oda **Müsait (IsAvailable=true)** yapılır |
| `3` | Completed | Tamamlandı | Oda **Müsait (IsAvailable=true)** yapılır |

**ERP İş Kuralı:** `TotalPrice = (CheckOutDate - CheckInDate).Days × Room.PricePerNight`

**İlişkiler:**
- `Reservations.RoomId` → `Rooms.Id` (N:1, RESTRICT)
- `Reservations.CustomerId` → `Customers.Id` (N:1, RESTRICT)
- `Reservations.Id` → `Payments.ReservationId` (1:N, CASCADE)

---

## 5. Employees Tablosu — Personel Yönetimi Modülü

> **ERP Modülü:** Personel / İnsan Kaynakları (HR)  
> **Açıklama:** Otel bünyesindeki çalışanları, görev pozisyonlarını ve maaş bilgilerini tutar.

| Sütun | Veri Tipi | Kısıtlama | Açıklama |
|---|---|---|---|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Personelin benzersiz kimliği |
| `FirstName` | TEXT | NOT NULL, MAX 50 | Personel adı |
| `LastName` | TEXT | NOT NULL, MAX 50 | Personel soyadı |
| `Position` | INTEGER | NOT NULL, ENUM | Görev pozisyonu (aşağıda açıklanmıştır) |
| `Salary` | REAL | NOT NULL, ≥ 0 | Aylık net maaş (TL) |
| `HireDate` | TEXT (DATE) | NOT NULL | İşe giriş tarihi |
| `HotelId` | INTEGER | FOREIGN KEY, NOT NULL | Çalıştığı otelin Id'si |
| `Email` | TEXT | NOT NULL, EMAIL FORMAT | İş e-posta adresi |
| `Phone` | TEXT | NOT NULL, PHONE FORMAT | İletişim telefonu |

**EmployeePosition ENUM Değerleri:**

| Değer | Kod | Türkçe Karşılığı |
|---|---|---|
| `0` | Manager | Müdür |
| `1` | Receptionist | Resepsiyonist |
| `2` | Housekeeping | Kat Hizmetleri |
| `3` | Chef | Aşçı |
| `4` | Security | Güvenlik |

**İlişkiler:**
- `Employees.HotelId` → `Hotels.Id` (N:1, CASCADE)

---

## 6. Payments Tablosu — Gelir ve Ödeme Yönetimi Modülü

> **ERP Modülü:** Gelir ve Finans Yönetimi (Muhasebe)  
> **Açıklama:** Rezervasyonlara ait ödeme tahsilatlarını tutar. Birden fazla taksit ödemesi eklenebilir. Dashboard'da toplam ciro bu tablodaki `Status=Paid` kayıtların toplamından hesaplanır.

| Sütun | Veri Tipi | Kısıtlama | Açıklama |
|---|---|---|---|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Ödemenin benzersiz kimliği |
| `ReservationId` | INTEGER | FOREIGN KEY, NOT NULL | İlişkili rezervasyonun Id'si |
| `Amount` | REAL | NOT NULL, > 0 | Ödeme tutarı (TL) |
| `PaymentDate` | TEXT (DATE) | NOT NULL | Ödemenin yapıldığı tarih |
| `PaymentMethod` | INTEGER | NOT NULL, ENUM | Ödeme yöntemi (aşağıda açıklanmıştır) |
| `Status` | INTEGER | NOT NULL, ENUM | Ödeme durumu (aşağıda açıklanmıştır) |

**PaymentMethod ENUM Değerleri:**

| Değer | Kod | Türkçe Karşılığı |
|---|---|---|
| `0` | CreditCard | Kredi Kartı |
| `1` | Cash | Nakit |
| `2` | BankTransfer | Banka Havalesi |

**PaymentStatus ENUM Değerleri:**

| Değer | Kod | Türkçe Karşılığı | Ciro Etkisi |
|---|---|---|---|
| `0` | Pending | Beklemede | Ciro'ya dahil edilmez |
| `1` | Paid | Ödendi | Toplam ciroya **eklenir** |
| `2` | Refunded | İade Edildi | Ciro'ya dahil edilmez |

**İlişkiler:**
- `Payments.ReservationId` → `Reservations.Id` (N:1, CASCADE)

---

## ERP Modül Özet Tablosu

| ERP Modülü | Veritabanı Tablosu | CRUD | Bağlı Modüller |
|---|---|---|---|
| 🏨 Otel Yönetimi | `Hotels` | ✅ Tam | Odalar, Personel |
| 🚪 Oda Yönetimi | `Rooms` | ✅ Tam | Oteller, Rezervasyonlar |
| 👤 Müşteri Yönetimi | `Customers` | ✅ Tam | Rezervasyonlar |
| 📅 Rezervasyon Yönetimi | `Reservations` | ✅ Tam | Odalar, Müşteriler, Ödemeler |
| 👷 Personel / İK Yönetimi | `Employees` | ✅ Tam | Oteller |
| 💰 Gelir & Finans Yönetimi | `Payments` | ✅ Tam | Rezervasyonlar |

---

## Veritabanı Bağlantı Bilgileri

| Özellik | Değer |
|---|---|
| **Veritabanı Motoru** | SQLite |
| **Bağlantı Dizesi** | `Data Source=HotelERP.db` |
| **Veritabanı Dosyası** | `HotelERP.db` (proje kök dizininde) |
| **ORM** | Entity Framework Core 9.0.2 |
| **Migration Tablosu** | `__EFMigrationsHistory` |
| **Hedef Framework** | .NET 9.0 |

---

## Migration Komutları

```bash
# Yeni bir migration oluştur
dotnet ef migrations add <MigrationAdi>

# Veritabanını güncelle (migrationları uygula)
dotnet ef database update

# Tüm migrationları listele
dotnet ef migrations list

# Son migrationı geri al
dotnet ef migrations remove
```
