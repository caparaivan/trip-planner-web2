# Trip Planner Web

Projekat za predmet Primena veb programiranja u infrastrukturnim sistemima.
Tema je web aplikacija za planiranje putovanja.

## Tehnologije

- Frontend: React + Vite, funkcionalne komponente, hooks, Context API.
- Backend: Microsoft Service Fabric, stateless i stateful servisi, Service Fabric Remoting.
- Baza: Microsoft SQL Server kroz Entity Framework Core i SQL migracije.

## Struktura

- `src/frontend` - React aplikacija.
- `src/backend/BackendSF` - stateless REST ulazni servis.
- `src/backend/TripPlanner.Contracts` - DTO modeli i remoting interfejsi.
- `src/backend/ValidatorService` - stateless validacija i poslovna pravila.
- `src/backend/ShareService` - stateful servis za share tokene kroz Reliable Dictionary.
- `src/backend/EventDispatcherService` - stateful servis za asinhrone dogadjaje kroz Reliable Queue.
- `src/backend/TripPlannerApplication` - Service Fabric manifesti.
- `docs` - biljeske iz PDF materijala i arhitektura.

## Pokretanje frontend-a

```powershell
cd src/frontend
npm install
Copy-Item .env.example .env
npm run dev
```

Backend URL se podesava u `src/frontend/.env` kroz `VITE_API_BASE_URL`.

## Pokretanje backend-a

Potrebno je lokalno okruzenje iz vjezbi:

- Visual Studio 2022 pokrenut kao Administrator.
- Workload: Azure development.
- Microsoft Service Fabric SDK i Runtime.
- Local Cluster Manager pokrenut kao 1-node ili 5-node cluster.
- SQL Server dostupan lokalno.

Backend solution je u `src/backend/TripPlanner.sln`. Service Fabric application manifesti su u `src/backend/TripPlannerApplication`.

Connection string je u `src/backend/BackendSF/appsettings.json`.

## Trenutno implementirano

Tacka 3.1 je pokrivena kroz CRUD za planove putovanja:

- `GET /api/planovi-putovanja`
- `GET /api/planovi-putovanja/{id}`
- `POST /api/planovi-putovanja`
- `PUT /api/planovi-putovanja/{id}`
- `DELETE /api/planovi-putovanja/{id}`

Tacka 3.2 je pokrivena kroz CRUD za destinacije u okviru izabranog plana putovanja:

- `GET /api/planovi-putovanja/{planPutovanjaId}/destinacije`
- `GET /api/planovi-putovanja/{planPutovanjaId}/destinacije/{id}`
- `POST /api/planovi-putovanja/{planPutovanjaId}/destinacije`
- `PUT /api/planovi-putovanja/{planPutovanjaId}/destinacije/{id}`
- `DELETE /api/planovi-putovanja/{planPutovanjaId}/destinacije/{id}`

Tacka 3.3 je pokrivena kroz CRUD za aktivnosti i kalendarski prikaz aktivnosti po danima:

- `GET /api/planovi-putovanja/{planPutovanjaId}/aktivnosti`
- `GET /api/planovi-putovanja/{planPutovanjaId}/aktivnosti/{id}`
- `POST /api/planovi-putovanja/{planPutovanjaId}/aktivnosti`
- `PUT /api/planovi-putovanja/{planPutovanjaId}/aktivnosti/{id}`
- `DELETE /api/planovi-putovanja/{planPutovanjaId}/aktivnosti/{id}`

Tacka 3.4 je pokrivena kroz CRUD za troskove i automatski pregled budzeta:

- `GET /api/planovi-putovanja/{planPutovanjaId}/troskovi`
- `GET /api/planovi-putovanja/{planPutovanjaId}/troskovi/pregled-budzeta`
- `GET /api/planovi-putovanja/{planPutovanjaId}/troskovi/{id}`
- `POST /api/planovi-putovanja/{planPutovanjaId}/troskovi`
- `PUT /api/planovi-putovanja/{planPutovanjaId}/troskovi/{id}`
- `DELETE /api/planovi-putovanja/{planPutovanjaId}/troskovi/{id}`

Frontend ima formu za kreiranje i izmjenu plana, listu planova, prikaz detalja, brisanje plana, formu, listu, izmjenu i brisanje destinacija, formu, listu i kalendar aktivnosti, kao i evidenciju troskova sa pregledom planiranog budzeta, ukupnih troskova, preostalog budzeta i troskova po kategorijama za odabrani plan.

## Migracije

Migracije se dodaju u `BackendSF`, jer taj projekat sadrzi `AppDbContext`:

```powershell
dotnet ef migrations add InitialCreate --project BackendSF --startup-project BackendSF
dotnet ef database update --project BackendSF --startup-project BackendSF
```

U Service Fabric/Visual Studio okruzenju ekvivalentno se koristi Package Manager Console uz `BackendSF` kao projekat koji sadrzi `DbContext`.
