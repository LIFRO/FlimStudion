# FilmHyrning API (ASP.NET Core 8, InMemory DB)

## Kom igång
1) Installera .NET SDK 8.
2) Kör `dotnet restore` (första gången).
3) Starta API: `dotnet run --project BackEnd`.
   - HTTP: http://localhost:5001
   - HTTPS: http://localhost:5000 (används nedan)
   - Datat lagras InMemory och nollställs vid omstart.

## Konfiguration
JWT finns i `BackEnd/appsettings.json` (Issuer/Audience/Key). CORS är öppet för `http://localhost:5500` och `http://127.0.0.1:5500`.

## Autentisering
- Registrera admin: `POST /api/users/register` med `IsAdmin:true`.
- Registrera filmstudio: `POST /api/filmstudio/register` med `UserName`, `Password`, `StudioName`, `City`.
- Logga in: `POST /api/users/authenticate` -> svar `{ dto, token }`.
- Skicka token i headern `Authentication: Bearer <jwt>` (API lyssnar på den headern).

## Endpoints (kortfattat)
- **Publikt**
  - `GET /api/films` – alla filmer, anonymt får bara Titel/Genre.
  - `GET /api/films/{id}` – specifik film, anonymt får bara Titel/Genre.
  - `GET /api/filmstudios` – lista; anonym får bara Id & namn.
  - `GET /api/filmstudios/{id}` – studio; full info endast admin/ägande studio.
- **Admin**
  - `POST /api/films` – skapa film + antal kopior.
  - `PATCH /api/films/{id}` – uppdatera titel/genre.
  - `POST /api/films/{id}` – ändra antal kopior (lägger till eller tar bort icke-utlånade).
- **Admin & Filmstudio**
  - `POST /api/films/rent?id={filmId}&studioId={studioId}` – hyr ut filmkopia.
  - `POST /api/films/return?id={filmId}&studioId={studioId}` – återlämna kopia.
- **Filmstudio**
  - `GET /api/mystudio/rentals` – se egna utlånade kopior.

## Snabbtest (REST Client)
```
@base = http://localhost:5001/api

### Logga in som admin (får token i svaret)
POST {{base}}/users/authenticate
Content-Type: application/json

{
  "userName": "Testadmin",
  "password": "P@ssw0rd!"
}

### Skapa film (klistra in token nedan)
@token = <JWT_HÄR>

POST {{base}}/films
Authentication: Bearer {{token}}
Content-Type: application/json

{
  "title": "Cool Shark",
  "genre": "Action",
  "copies": 3
}
