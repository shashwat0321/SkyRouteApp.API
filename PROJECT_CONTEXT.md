# SkyRoute API - Complete Project Context

## WHO IS READING THIS
This document is for a new AI assistant helping continue development of the SkyRoute API project. The developer is learning ASP.NET Core while building a production-quality flight booking system. Always explain concepts clearly, go one step at a time, never overwhelm with too much at once, and always justify architectural decisions.

---

## PROJECT OVERVIEW

**Name:** SkyRoute  
**Type:** Flight Booking REST API  
**Stack:** ASP.NET Core (.NET 10), EF Core 10, SQL Server, Swagger  
**IDE:** Visual Studio  
**Repo:** SkyRouteApp.API | Branch: Experiments  
**Database:** SQL Server (SSMS) | DB Name: SkyRouteDb

---

## ARCHITECTURE - CLEAN ARCHITECTURE (4 PROJECTS)

```
SkyRoute.API           → Presentation Layer (Controllers, Program.cs)
SkyRoute.Application   → Business Logic (Services, DTOs, Interfaces, Mappers, Providers, Helpers)
SkyRoute.Infrastructure → External Concerns (DbContext, Repositories, EF Configurations, Migrations)
SkyRoute.Domain        → Core Domain (Entities, Enums, BaseEntity) — NO dependencies
```

### Dependency Flow
```
API → Application → Domain
Infrastructure → Application + Domain
Domain → NOTHING (pure, no dependencies)
```

### Key Rules
- Repository **interfaces** live in `Application/Interfaces/Repositories/`
- Service **interfaces** live in `Application/Interfaces/Services/`
- Provider **interface** lives in `Application/Interfaces/IFlightProvider.cs`
- Provider **implementations** live in `Application/Providers/`
- EF Core **configurations** are separate files in `Infrastructure/Data/Configurations/`
- DbContext uses `ApplyConfigurationsFromAssembly()` — auto-discovers all configs, no explicit registration needed
- Connection string in `appsettings.Development.json` (gitignored, held locally)
- `Application` project must NEVER reference EF Core — all `ToListAsync()` / `FirstOrDefaultAsync()` calls stay in Infrastructure repositories

---

## DOMAIN MODEL - ALL ENTITIES

### BaseEntity (Abstract)
```
Location: Domain/Common/BaseEntity.cs
Fields: Id (int), CreatedAt (DateTime), UpdatedAt (DateTime)
Inherited by: ALL entities
```

### User
```
Location: Domain/Entities/User.cs
Fields: Email, PasswordHash, FirstName, LastName, Role (string: "Admin" or "Customer")
Navigation: List<Booking> Bookings
Auth: Manual JWT planned (NOT ASP.NET Core Identity) — to learn JWT from scratch
```

### Airline
```
Location: Domain/Entities/Airline.cs
Fields: Name, Code (IATA-style, e.g. "GW", "BW")
Navigation: List<FlightSchedule> Schedules
IMPORTANT: Airline.Code is the key used to select the correct pricing provider at runtime
Seeded data: GlobalWings (GW), BudgetWings (BW)
NOTE: BudgetWings may still be seeded as "SkyTravel" with Code "ST" in the database
if not manually updated. The C# provider uses "BW". Fix with:
  UPDATE Airlines SET Name = 'BudgetWings', Code = 'BW' WHERE Code = 'ST';
```

### Airport
```
Location: Domain/Entities/Airport.cs
Fields: Code (IATA: "DEL"), Name, City, Country
Navigation: NONE — Airport does not track flights back
International check: derived by comparing DepartureAirport.Country != ArrivalAirport.Country
Seeded data: DEL, BOM, BLR, CCU, HYD
```

### FlightSchedule (THE RECURRING TEMPLATE)
```
Location: Domain/Entities/FlightSchedule.cs
Purpose: A recurring flight pattern — like a train timetable entry. Does NOT represent a
         specific date. Think of it as "this flight runs Mon/Wed/Fri between these airports".
Fields:
  - FlightNumber (string)
  - AirlineId + Airline (nav)
  - DepartureAirportId + DepartureAirport (nav)
  - ArrivalAirportId + ArrivalAirport (nav)
  - DepartureTime (TimeOnly — time only, NO date stored)
  - ArrivalTime (TimeOnly — time only, NO date stored)
  - OperatingDays ([Flags] enum — which days of the week it operates)
  - EffectiveFrom, EffectiveTo (DateTime — the date window this schedule is valid)
  - EconomySeats, BusinessSeats, FirstClassSeats (int — total capacity)
  - EconomyBasePrice, BusinessBasePrice, FirstClassBasePrice (decimal — raw base prices per cabin)
  - Status (ScheduleStatus enum)
Navigation: List<Flight> Flights
NOTE: Base prices are stored here. Final prices are NEVER stored — always computed live by providers.
```

### Flight (THE CONCRETE DATED INSTANCE)
```
Location: Domain/Entities/Flight.cs
Purpose: One specific date's occurrence of a FlightSchedule. Created on demand when a user
         selects a schedule for a date. NOT pre-populated.
Fields:
  - FlightScheduleId + FlightSchedule (nav)
  - FlightDate (DateTime — the specific operating date)
  - EconomyAvailable, BusinessAvailable, FirstClassAvailable (int — decremented on booking)
  - Status (FlightStatus enum — for this specific date only)
  - RowVersion (byte[] — EF Core concurrency token, prevents overbooking)
Navigation: List<Booking> Bookings
Unique constraint: (FlightScheduleId, FlightDate) — enforced at DB level, prevents duplicates
NO price fields — prices are computed live via providers
```

### Booking
```
Location: Domain/Entities/Booking.cs
Fields:
  - UserId + User (nav)
  - FlightId + Flight (nav) — links to the DATED INSTANCE, not the schedule
  - CabinClass (CabinClass enum)
  - NumberOfSeats (int)
  - TotalPrice (decimal — FROZEN price snapshot at booking time, never changes)
  - Status (BookingStatus enum)
  - PaymentStatus (PaymentStatus enum)
Navigation: List<Passenger> Passengers
```

### Passenger
```
Location: Domain/Entities/Passenger.cs
Fields:
  - BookingId + Booking (nav)
  - FirstName, LastName, DateOfBirth
  - PassportNumber (nullable — required for international flights)
  - NationalId (nullable — required for domestic flights)
  - Nationality
Stored permanently for legal/audit reasons
International vs domestic: DepartureAirport.Country != ArrivalAirport.Country
```

---

## ENUMS

```csharp
CabinClass     { Economy, Business, FirstClass }
ScheduleStatus { Active, Suspended, Cancelled }
FlightStatus   { Scheduled, Delayed, Cancelled, Completed }
BookingStatus  { Pending, Confirmed, Cancelled }
PaymentStatus  { Unpaid, Paid, Refunded }

[Flags]
OperatingDays  { None=0, Monday=1, Tuesday=2, Wednesday=4, Thursday=8,
                 Friday=16, Saturday=32, Sunday=64 }
```

### OperatingDays — How [Flags] Works
Powers of 2 mean each day occupies one bit. Multiple days are stored as a single integer.
```
Monday(1) | Wednesday(4) | Friday(16) = 21 stored in DB
Check:  (schedule.OperatingDays & OperatingDays.Wednesday) != 0
```
`DateHelper.ConvertDayToFlag(DayOfWeek)` converts a `DayOfWeek` to the correct flag.
Location: `Application/Common/Helpers/DateHelper.cs` — namespace: `SkyRoute.Application.Common.Helpers`

---

## NAVIGATION PROPERTIES MAP

```
Airline         → List<FlightSchedule>     (1:Many)
FlightSchedule  → Airline                  (Many:1)
FlightSchedule  → DepartureAirport         (Many:1)
FlightSchedule  → ArrivalAirport           (Many:1)
FlightSchedule  → List<Flight>             (1:Many)
Flight          → FlightSchedule           (Many:1)
Flight          → List<Booking>            (1:Many)
Booking         → User                     (Many:1)
Booking         → Flight                   (Many:1)
Booking         → List<Passenger>          (1:Many)
Passenger       → Booking                  (Many:1)
User            → List<Booking>            (1:Many)
Airport         → NONE
```

---

## PROVIDER / PRICING DESIGN

### The Four Tiers of Price
```
Base price   → stored on FlightSchedule (EconomyBasePrice etc.)  — stable, in DB
Provider rule → code in Application/Providers/                   — swappable strategy
Final price  → computed live, placed in DTO                      — never stored
Booked price → Booking.TotalPrice                                — frozen snapshot
```

### Interface
```csharp
// Location: Application/Interfaces/IFlightProvider.cs
public interface IFlightProvider
{
    string AirlineCode { get; }
    decimal CalculatePrice(decimal basePrice, CabinClass cabinClass);
}
```
`CabinClass` is in the signature for future providers that apply different rules per cabin.
Current providers ignore it — the correct base price for the cabin is passed by the caller.

### Implementations

```csharp
// Application/Providers/GlobalWingsProvider.cs
// AirlineCode: "GW"
// Rule: base fare + 15% fuel surcharge, rounded to 2 decimal places
return Math.Round(basePrice * 1.15m, 2);

// Application/Providers/BudgetWingsProvider.cs
// AirlineCode: "BW" (DB may still have "ST" — see Airlines note above)
// Rule: base fare minus 10% promotional discount, minimum final price $29.99
return Math.Max(basePrice * 0.90m, 29.99m);
```

### DI Registration — IEnumerable Pattern
Both providers are registered under the same interface. DI hands the service ALL of them.
```csharp
// Program.cs
builder.Services.AddScoped<IFlightProvider, GlobalWingsProvider>();
builder.Services.AddScoped<IFlightProvider, BudgetWingsProvider>();

// Service receives all providers
public FlightService(..., IEnumerable<IFlightProvider> providers) { ... }

// Service picks the right one per flight
var provider = _providers.First(p => p.AirlineCode == dto.AirlineCode);
```
Adding a third provider = one new class + one AddScoped line. Service never changes.

KNOWN RISK: `_providers.First(...)` throws `InvalidOperationException` if no provider matches
the airline code. Should use `FirstOrDefault` with a meaningful fallback error. Not yet fixed.

### When Prices Are Calculated
```
Schedule search → CalculatePrice called ONCE per result (economy only)
Flight detail   → CalculatePrice called 3 times (economy, business, first class) via ApplyPrices()
Booking         → price already shown to user, frozen into Booking.TotalPrice
```

---

## REPOSITORY PATTERN

Three separate repositories — one per concern.

### IScheduleRepository
```csharp
// Location: Application/Interfaces/Repositories/IScheduleRepository.cs
Task<IEnumerable<FlightSchedule>> SearchSchedulesAsync(string origin, string destination, DateOnly date);
Task<FlightSchedule?> GetScheduleByIdAsync(int scheduleId);
```

### ScheduleRepository (Implementation)
```
Location: Infrastructure/Repositories/ScheduleRepository.cs

SearchSchedulesAsync:
  - Converts DateOnly → DateTime, derives OperatingDays flag via DateHelper
  - Filters by origin/destination airport codes
  - Filters by EffectiveFrom <= date <= EffectiveTo
  - Filters by OperatingDays bitwise check
  - Includes Airline, DepartureAirport, ArrivalAirport navigations
  - Executes as ONE DB query via ToListAsync()

GetScheduleByIdAsync:
  - Includes Airline, DepartureAirport, ArrivalAirport
  - Returns null if not found
```

### IFlightRepository
```csharp
// Location: Application/Interfaces/Repositories/IFlightRepository.cs
Task<Flight?> GetFlightAsync(int scheduleId, DateTime flightDate);
Task<Flight?> GetFlightByIdAsync(int flightId);
Task<Flight> AddFlightAsync(Flight flight);
```

### FlightRepository (Implementation)
```
Location: Infrastructure/Repositories/FlightRepository.cs

GetFlightAsync:
  - Looks up by (FlightScheduleId, FlightDate.Date)
  - Includes full FlightSchedule navigation (Airline, Airports via ThenInclude)

GetFlightByIdAsync:
  - Looks up by Flight.Id
  - Includes full FlightSchedule navigation (Airline, Airports via ThenInclude)

AddFlightAsync:
  - Adds Flight to context and saves
  - Will throw DbUpdateException if unique constraint (ScheduleId, FlightDate) is violated
```

### IAirportRepository
```csharp
// Location: Application/Interfaces/Repositories/IAirportRepository.cs
Task<IEnumerable<Airport>> GetAllAirportsAsync();
```

### AirportRepository (Implementation)
```
Location: Infrastructure/Repositories/AirportRepository.cs
GetAllAirportsAsync: returns all Airport rows via ToListAsync()
```

---

## SERVICE LAYER

Three separate services — one per concern.

### IScheduleService / ScheduleService
```csharp
// Interface: Application/Interfaces/Services/IScheduleService.cs
Task<IEnumerable<ScheduleSearchResultDTO>> SearchSchedulesAsync(ScheduleSearchRequest request);
```
```
Implementation: Application/Services/ScheduleService.cs
Dependencies: IScheduleRepository, IMapper, IEnumerable<IFlightProvider>

SearchSchedulesAsync:
1. Calls repo.SearchSchedulesAsync — returns materialised List<FlightSchedule>
2. AutoMapper maps all structural fields → List<ScheduleSearchResultDTO>
3. Index-based for loop iterates both lists together:
   - Stamps real travel date onto DepartureDateTime (was DateTime.MinValue + time from mapper)
   - Stamps real travel date onto ArrivalDateTime
   - OVERNIGHT FIX: if ArrivalDateTime <= DepartureDateTime → add 1 day to arrival
   - Picks provider by dto.AirlineCode, calls CalculatePrice(schedule.EconomyBasePrice, Economy)
   - Sets dto.EconomyPrice
```

### IFlightService / FlightService
```csharp
// Interface: Application/Interfaces/Services/IFlightService.cs
Task<FlightDTO> CreateFlightAsync(CreateFlightRequest request);
Task<FlightDTO> GetFlightByIdAsync(int flightId);
```
```
Implementation: Application/Services/FlightService.cs
Dependencies: IFlightRepository, IScheduleRepository, IMapper, IEnumerable<IFlightProvider>

CreateFlightAsync:
1. Loads FlightSchedule via IScheduleRepository (throws KeyNotFoundException if not found)
2. Validates: date not in the past, date within EffectiveFrom..EffectiveTo, correct operating day
3. Checks for existing flight instance (throws InvalidOperationException if duplicate)
4. Creates Flight entity — copies seat counts from schedule, Status = Scheduled
5. Saves to DB via AddFlightAsync — unique constraint is the final race guard
6. Sets FlightSchedule nav on returned entity, maps to FlightDTO
7. Manually stamps DepartureDateTime + ArrivalDateTime from FlightDate + Schedule times
8. Overnight fix applied
9. Calls ApplyPrices() — sets all three cabin prices via provider

GetFlightByIdAsync:
1. Loads Flight with all includes via IFlightRepository (throws KeyNotFoundException if not found)
2. Maps to FlightDTO
3. Manually stamps DepartureDateTime + ArrivalDateTime from FlightDate + Schedule times
4. Overnight fix applied
5. Calls ApplyPrices() — sets all three cabin prices via provider

Private ApplyPrices(FlightDTO dto, FlightSchedule schedule):
  provider = _providers.First(p => p.AirlineCode == dto.AirlineCode)
  dto.EconomyPrice    = provider.CalculatePrice(schedule.EconomyBasePrice,    CabinClass.Economy)
  dto.BusinessPrice   = provider.CalculatePrice(schedule.BusinessBasePrice,   CabinClass.Business)
  dto.FirstClassPrice = provider.CalculatePrice(schedule.FirstClassBasePrice, CabinClass.FirstClass)
  Each cabin uses its OWN base price — not the same base price three times.
```

### IAirportService / AirportService
```csharp
// Interface: Application/Interfaces/Services/IAirportService.cs
Task<IEnumerable<AirportResponse>> GetAirportsAsync();
```
```
Implementation: Application/Services/AirportService.cs
Dependencies: IAirportRepository, IMapper
GetAirportsAsync: fetches all airports from repo, maps to IEnumerable<AirportResponse>
```

---

## MAPPING STRATEGY — HYBRID

AutoMapper handles structural field mapping (~70%).
Service manually handles computed fields: dates and prices (~30%).
Never try to inject providers or business logic into AutoMapper profiles.

### MappingProfile
```
Location: Application/Mappers/MappingProfile.cs

FlightSchedule → ScheduleSearchResultDTO:
  ScheduleId          ← src.Id
  FlightNumber        ← src.FlightNumber
  AirlineName         ← src.Airline.Name
  AirlineCode         ← src.Airline.Code
  DepartureAirportCode← src.DepartureAirport.Code
  DepartureCity       ← src.DepartureAirport.City
  ArrivalAirportCode  ← src.ArrivalAirport.Code
  ArrivalCity         ← src.ArrivalAirport.City
  DepartureDateTime   ← DateTime.MinValue + DepartureTime (placeholder time, service fixes date)
  ArrivalDateTime     ← DateTime.MinValue + ArrivalTime   (placeholder time, service fixes date)
  EconomyPrice        ← IGNORED (set by service via provider)

Flight → FlightDTO:
  FlightId            ← src.Id
  ScheduleId          ← src.FlightScheduleId
  FlightNumber        ← src.FlightSchedule.FlightNumber
  AirlineName         ← src.FlightSchedule.Airline.Name
  AirlineCode         ← src.FlightSchedule.Airline.Code
  DepartureAirportCode← src.FlightSchedule.DepartureAirport.Code
  DepartureCity       ← src.FlightSchedule.DepartureAirport.City
  ArrivalAirportCode  ← src.FlightSchedule.ArrivalAirport.Code
  ArrivalCity         ← src.FlightSchedule.ArrivalAirport.City
  EconomyAvailable    ← src.EconomyAvailable
  BusinessAvailable   ← src.BusinessAvailable
  FirstClassAvailable ← src.FirstClassAvailable
  Status              ← src.Status
  DepartureDateTime   ← IGNORED (set manually in service)
  ArrivalDateTime     ← IGNORED (set manually in service)
  EconomyPrice        ← IGNORED (set manually via ApplyPrices)
  BusinessPrice       ← IGNORED (set manually via ApplyPrices)
  FirstClassPrice     ← IGNORED (set manually via ApplyPrices)

Airport → AirportResponse:
  Code    ← src.Code
  Name    ← src.Name
  City    ← src.City
  Country ← src.Country
```

---

## DTOs

### ScheduleSearchRequest (INPUT — schedule search)
```csharp
// Application/DTOs/Request/ScheduleSearchRequest.cs
[Required] string Origin       // "DEL"
[Required] string Destination  // "BOM"
[Required] DateOnly Date       // 2026-06-25
```

### CreateFlightRequest (INPUT — create a flight instance)
```csharp
// Application/DTOs/Request/CreateFlightRequest.cs
[Required] int ScheduleId
[Required] DateOnly FlightDate
```

### ScheduleSearchResultDTO (OUTPUT — schedule search results list)
```csharp
// Application/DTOs/Response/ScheduleSearchResultDTO.cs
int ScheduleId
string FlightNumber
string AirlineName
string AirlineCode
string DepartureAirportCode
string DepartureCity
DateTime DepartureDateTime   // computed: requestDate + DepartureTime
string ArrivalAirportCode
string ArrivalCity
DateTime ArrivalDateTime     // computed: requestDate + ArrivalTime (+ 1 day if overnight)
decimal EconomyPrice         // computed by provider — NOT the raw base price
```

### FlightDTO (OUTPUT — single flight instance, used by both CreateFlight and GetFlightById)
```csharp
// Application/DTOs/Response/FlightDTO.cs
int FlightId
int ScheduleId
string FlightNumber
string AirlineName
string AirlineCode
string DepartureAirportCode
string DepartureCity
DateTime DepartureDateTime
string ArrivalAirportCode
string ArrivalCity
DateTime ArrivalDateTime
int EconomyAvailable
int BusinessAvailable
int FirstClassAvailable
decimal EconomyPrice         // computed by provider
decimal BusinessPrice        // computed by provider
decimal FirstClassPrice      // computed by provider
FlightStatus Status
```

### AirportResponse (OUTPUT — airport list)
```csharp
// Application/DTOs/Response/AirportResponse.cs
string Code
string Name
string City
string Country
```

---

## CONTROLLERS

### SchedulesController
```
Location: API/Controllers/SchedulesController.cs
Route: api/schedules
Dependencies: IScheduleService
```

**GET api/schedules/search** `[HttpGet("search")]`
- Accepts `ScheduleSearchRequest` via `[FromQuery]`
- Returns 200 with list, 400 if model invalid, 404 if no results

### FlightsController
```
Location: API/Controllers/FlightController.cs
Route: api/flights
Dependencies: IFlightService
```

**GET api/flights/{id}** `[HttpGet("{id}")]`
- Returns 200 with FlightDTO

**POST api/flights/create** `[HttpPost("create")]`
- Accepts `CreateFlightRequest` via `[FromBody]`
- Returns 201 with FlightDTO on success
- NOTE: No try-catch in either endpoint — exceptions bubble up unhandled until global
  exception middleware is added. TODO: Add `[Authorize(Roles = "Admin")]` on create
  when JWT is implemented.

### AirportController
```
Location: API/Controllers/AirportController.cs
Route: api/airport   (singular — class is AirportController, not AirportsController)
Dependencies: IAirportService
```

**GET api/airport** `[HttpGet]`
- Returns 200 with all airports

---

## THE CORE FLOW

### Step 1: SEARCH (implemented)
```
GET /api/schedules/search?origin=DEL&destination=BOM&date=2026-06-25
  ↓ SchedulesController → ScheduleService.SearchSchedulesAsync
  ↓ ScheduleRepository queries FlightSchedules (one DB query, all filters applied)
  ↓ AutoMapper maps structural fields → ScheduleSearchResultDTO list
  ↓ Service for-loop: fix dates, apply overnight logic, calculate EconomyPrice via provider
  ↓ Returns List<ScheduleSearchResultDTO>
```

### Step 2: FLIGHT INSTANCE CREATION (implemented — admin only when auth added)
```
POST /api/flights/create  { scheduleId: 3, flightDate: "2026-06-25" }
  ↓ FlightsController → FlightService.CreateFlightAsync
  ↓ Validates: schedule exists, date not past, date in range, correct operating day, no duplicate
  ↓ Creates Flight entity, saves to DB
  ↓ Unique constraint on (FlightScheduleId, FlightDate) is the final race guard
  ↓ Returns FlightDTO with seat availability + all 3 cabin prices
```

### Step 3: FLIGHT DETAIL (implemented)
```
GET /api/flights/{id}
  ↓ FlightsController → FlightService.GetFlightByIdAsync
  ↓ Loads Flight with full navigation includes
  ↓ Maps to FlightDTO, stamps dates, calls ApplyPrices (3 provider calls)
  ↓ Returns FlightDTO — all 3 prices + seat availability per cabin
```

### Step 4: BOOKING (NOT YET IMPLEMENTED)
```
User selects cabin class and submits booking
  ↓ Check availability for chosen cabin
  ↓ Decrement Flight.XxxAvailable using RowVersion concurrency token
  ↓ Snapshot: Booking.TotalPrice = provider.CalculatePrice(basePrice, cabinClass)
  ↓ Create Booking + Passengers
  ↓ Validate passport (international) or nationalId (domestic)
```

---

## CONCURRENCY

### The Problem
Two users select the same schedule + date simultaneously. Both check DB, find no Flight
instance, both try to create one. Must end up with exactly ONE instance.

### The Solution (partially implemented)
1. Unique index on `(FlightScheduleId, FlightDate)` — DB prevents duplicate Flight rows
2. `RowVersion` on Flight — optimistic concurrency for seat decrement (prevents overbooking)
3. Current create-flight flow: try INSERT → if DbUpdateException → return 409
4. Planned get-or-create: try INSERT → if unique violation → SELECT existing → return it

RowVersion-based overbooking protection (for booking step) is not yet implemented.

---

## PROGRAM.CS — DI REGISTRATIONS

```csharp
builder.Services.AddDbContext<SkyRouteDbContext>(...);
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();

builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IAirportService, AirportService>();

builder.Services.AddScoped<IFlightProvider, GlobalWingsProvider>();
builder.Services.AddScoped<IFlightProvider, BudgetWingsProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

Middleware order:
```csharp
app.UseSwagger() / app.UseSwaggerUI()  // Development only
app.UseHttpsRedirection()
app.UseRouting()
app.MapControllers()
```
NOTE: No authentication middleware yet. No global exception middleware yet — when added,
register it before `UseRouting`.

---

## EF CORE CONFIGURATIONS

All configurations are in `Infrastructure/Data/Configurations/`, auto-discovered via
`ApplyConfigurationsFromAssembly`. Key points:
- `AirlineConfiguration`: Code has unique index
- `AirportConfiguration`: Code has unique index
- `FlightScheduleConfiguration`: base prices typed as `decimal(18,2)`, all FKs Restrict
- `FlightConfiguration`: RowVersion configured as concurrency token,
  unique index on `(FlightScheduleId, FlightDate)`, Bookings FK Restrict

---

## NUGET PACKAGES

```
SkyRoute.API:
  - Microsoft.EntityFrameworkCore.Design
  - Swashbuckle.AspNetCore

SkyRoute.Application:
  - AutoMapper.Extensions.Microsoft.DependencyInjection
  - FluentValidation.DependencyInjectionExtensions (not used yet)

SkyRoute.Infrastructure:
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore (not used yet)

SkyRoute.Domain:
  - No packages (pure domain)
```

---

## WHAT IS IMPLEMENTED

### ✅ Complete:
- Clean Architecture 4-project solution
- All domain entities + enums
- EF Core configurations (separate files, auto-discovered)
- Database: all tables created, migration applied (InitialCreate)
- Seed data: Airlines (GW, BW/ST), Airports (DEL, BOM, BLR, CCU, HYD), FlightSchedules
- IScheduleRepository + ScheduleRepository (SearchSchedulesAsync, GetScheduleByIdAsync)
- IFlightRepository + FlightRepository (GetFlightAsync, GetFlightByIdAsync, AddFlightAsync)
- IAirportRepository + AirportRepository (GetAllAirportsAsync)
- IScheduleService + ScheduleService (SearchSchedulesAsync)
- IFlightService + FlightService (CreateFlightAsync, GetFlightByIdAsync)
- IAirportService + AirportService (GetAirportsAsync)
- IFlightProvider + GlobalWingsProvider + BudgetWingsProvider
- AutoMapper MappingProfile (FlightSchedule→ScheduleSearchResultDTO, Flight→FlightDTO, Airport→AirportResponse)
- DateHelper.ConvertDayToFlag
- SchedulesController — GET /api/schedules/search
- FlightsController — GET /api/flights/{id}, POST /api/flights/create
- AirportController — GET /api/airport
- Overnight flight date fix (ArrivalDateTime correctly rolls to next day)
- EconomyPrice in schedule search results (via provider)
- All 3 cabin prices in flight detail/create responses (via ApplyPrices)
- Swagger (Development only)
- All DI registrations wired up

### ❌ Not Yet Implemented:
- Fix `_providers.First()` crash — replace with `FirstOrDefault` + meaningful error
- Full get-or-create concurrency pattern (currently just create + catch conflict)
- RowVersion-based seat overbooking protection
- Custom exception types (NotFoundException, DuplicateFlightException, etc.)
- Global exception handling middleware (controllers currently have NO try-catch)
- JWT Authentication (register, login, token validation)
- Booking endpoint (create booking + passengers + validation)
- Cancellation logic (add seats back)
- FluentValidation for inputs
- Serilog logging
- Payment integration (mock Stripe)

---

## IMMEDIATE NEXT STEPS (in order)

1. Update DB: `UPDATE Airlines SET Name = 'BudgetWings', Code = 'BW' WHERE Code = 'ST';`
2. Custom exception types — NotFoundException, DuplicateFlightException in Application/Common/Exceptions/
3. Global exception handling middleware — maps exception types to HTTP status codes, uses ProblemDetails format (RFC 9457); register before UseRouting in Program.cs
4. Fix `_providers.First()` — use FirstOrDefault, throw NotFoundException (ties into step 2/3)
5. JWT Authentication — register, login, token generation/validation
6. Booking endpoint — create booking with passengers, decrement seats, snapshot price
7. FluentValidation — validate all request DTOs
8. Serilog logging
9. Payment integration (mock Stripe)

---

## DEVELOPER LEARNING PREFERENCES (IMPORTANT)

- Explain ONE concept at a time — never overwhelm
- Always explain WHY before HOW
- Always mention alternatives even if not using them
- Be STRICT and HONEST — don't be biased toward developer's ideas, push back when needed
- Go SLOW — confirm understanding before moving forward
- No walls of text — short, clear explanations
- Never assume knowledge — explain everything
- Developer has strong C# background but is learning:
  EF Core, JWT, Clean Architecture, AutoMapper, Repository pattern, DI, REST API design

---

**END OF CONTEXT DOCUMENT**
