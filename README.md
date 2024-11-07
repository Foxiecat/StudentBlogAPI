# Student Blog API

En blogg-plattform API bygget med ASP.NET Core som gir mulighet for å administrere og/eller lage brukere, innlegg, og kommentarer.

## Databasestruktur

Databasen er satt opp i MySQL og består av følgende tabeller:

1. **Users**: Inneholder informasjon om brukere.
    - **Id** (GUID): Primærnøkkel.
    - **UserName** (string): Unikt brukernavn.
    - **Email** (string): Unik e-postadresse.
    - **FirstName** og **LastName** (string): Navn på brukeren.
    - **HashedPassword** (string): Hashet passord.
    - **IsAdminUser** (bool): Indikerer om brukeren er administrator.
    - **Created** og **Updated** (DateTime): Tidspunkter for opprettelse og oppdatering.

2. **Posts**: Inneholder blogginnlegg.
    - **Id** (GUID): Primærnøkkel.
    - **UserId** (GUID): Fremmednøkkel som refererer til en bruker.
    - **Title** (string): Tittel på innlegget.
    - **Content** (string): Innholdet i innlegget.
    - **Created** og **Updated** (DateTime): Tidspunkter for opprettelse og oppdatering.

3. **Comments**: Inneholder kommentarer til innlegg.
    - **Id** (GUID): Primærnøkkel.
    - **PostId** (GUID): Fremmednøkkel som refererer til et innlegg.
    - **UserId** (GUID): Fremmednøkkel som refererer til en bruker.
    - **Content** (string): Kommentarens innhold.
    - **Created** og **Updated** (DateTime): Tidspunkter for opprettelse og oppdatering.

## Oppsett av database

1. **Opprettelse av database**:
    - Sørg for at MySQL-serveren kjører.
    - Gå inn i "Extensions/DatabaseServiceExtension.cs" og endre MySqlServerVersion til din versjon av MySQL: 
    ```csharp
    MySqlServerVersion(new Version(8, 4, 2))));
   ```
   - Gå inn i "Database/Database SQL" Setup og kjør StudentBlogDatabase.sql

## API-endepunkter

### Bruker-endepunkter (Users)

- **POST /api/v1/users**: Registrerer en ny bruker.
- **GET /api/v1/users/{id}**: Henter informasjon om en bruker basert på ID.
- **GET /api/v1/users?pageNumber=1&pageSize=10**: Henter en alle brukere.
- **GET /api/v1/users/{id}/posts**: Henter alle poster for denne brukeren
- **PUT /api/v1/users/{id}**: Oppdaterer brukerdata (kun for pålogget bruker).
- **DELETE /api/v1/users/{id}**: Sletter en bruker (kun for pålogget bruker).

### Innlegg-endepunkter (Posts)

- **POST /api/v1/posts**: Oppretter et nytt innlegg.
- **GET /api/v1/posts/{postId}**: Henter et innlegg basert på ID.
- **GET /api/v1/posts?pageNumber=1&pageSize=10**: Henter en alle innlegg.
- **PUT /api/v1/posts/{postId}**: Oppdaterer et innlegg (kun for innleggets eier).
- **DELETE /api/v1/posts/{postId}**: Sletter et innlegg (kun for innleggets eier).

### Kommentarer-endepunkter (Comments)

- **POST /api/v1/comments/{postId}**: Legger til en kommentar på et innlegg.
- **GET /api/v1/comments?pageNumber=1&pageSize=10**: Henter alle kommentarer
- **GET /api/v1/comments/{postId}/comments**: Henter alle kommentarer fra ett innlegg
- **PUT /api/v1/comments/{commentId}**: Oppdaterer en kommentar (kun for kommentarens eier).~~~~
- **DELETE /api/v1/comments/{commentId}**: Sletter en kommentar (kun for kommentarens eier).
---