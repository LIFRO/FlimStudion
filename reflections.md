# Reflections

## REST
- **Resurser & URL-design:** Filmer (`/api/films`), filmstudios (`/api/filmstudios`), uthyrningar (POST på `/api/films/rent`/`return`), användare (`/api/users`, `/api/filmstudio`). Resursnamn är substantiv, metoder styr verbet (GET/POST/PATCH).
- **Representationer:** Anonyma svar maskerar fält (t.ex. film listar bara Titel/Genre; studios visar bara Id/Namn). Autentiserade får fulla modeller. Det uppfyller kravet på minst två nivåer av exponering.
- **Statuskoder & fel:** BadRequest vid ogiltig input, Unauthorized/403 vid rollfel, NotFound när resurser saknas, Conflict vid affärsregler (t.ex. kopior saknas). Det gör API:t självbeskrivande.
- **Affärsflöden:** Admin skapar/ändrar filmer och kopior; filmstudio kan hyra/återlämna; admin kan också hyra/återlämna för test/support. `GET /api/mystudio/rentals` ger studios egna lån utan att avslöja andra.

## Implementation
- **Interna modeller:** `Film`, `FilmCopy`, `FilmStudio`, `User` + interfaces (`IFilm`, `IFilmCopy`, `IFilmStudio`, `IUser` etc.) för tydliga kontrakt och testbarhet.
- **DTO/requests:** `CreateFilm` används vid skapande, `UserRegisterDto` och `UserLoginDto` för indata. Svar använder `AdminUserDto`, `FilmStudioDto`, `FilmStudioUserDto` för att dölja känsliga fält (lösenhash, salt, kopior).
- **Skillnader resurs vs modell:** Publika svar utesluter `FilmCopies` och studio-detaljer om klienten är anonym; filmstudio-svar inkluderar endast egna lån. Interna modeller har navigationslistor och saltade hash; dessa exkluderas i DTO:er.
- **Lager:** Controllers → Services (`IFilmService`, `IUserService`) → Repositories (`IFilmRepository`, `IUserRepository`) → EF Core InMemory. DI gör det lätt att byta datakälla och testa.

## Säkerhet
- **Autentisering:** JWT Bearer med issuer/audience från `appsettings.json`. Token skapas i `UserService` och innehåller `NameIdentifier`, `Name`, `Role`. Giltighet: 60 min.
- **Auktorisation:** `[Authorize(Roles="admin")]` för filmadministration; `[Authorize(Roles="admin, filmstudio")]` för hyra/returnera; `[Authorize(Roles="filmstudio")]` för egna lån. `AdminController` verifierar dessutom att filmstudio-användare bara agerar på sitt eget `studioId` (Claim check + `StudioIdCheck`).
- **Dataexponering:** Publika endpoints returnerar reducerade projectioner; studios och filmer laddas med Include men trimmas innan svar till icke-behöriga. Kopior döljs vid filmlista för anonyma.
- **Lösenord:** Argon2id med unik 16-byte salt per användare; hash lagras i databasen. Ingen klartext sparas.
- **Transport & CORS:** HTTPS-port 5001; CORS begränsad till lokala dev-origins för frontenden.
- **Session/Logout i klient:** Token lagras klient-side (t.ex. localStorage/memory). Logout = ta bort token och sluta skicka `Authentication`-headern; servern nekar när token saknas/utgått. Ingen server-side session behöver stängas.
