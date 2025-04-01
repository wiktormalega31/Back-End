# Instrukcja instalacji

Aby zainstalować wszystkie wymagane moduły, wykonaj poniższe kroki:

1. Upewnij się, że masz zainstalowane .NET SDK oraz narzędzie `dotnet`.

2. Przejdź do katalogu projektu:

   ```bash
   cd c:\Users\%USER%\Desktop\PSK\Back-End
   ```

3. Zainstaluj wszystkie wymagane pakiety, korzystając z pliku `requirements.txt`:

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
   dotnet add package BCrypt.Net-Next
   dotnet add package Swashbuckle.AspNetCore
   ```

4. Po zainstalowaniu pakietów uruchom aplikację:

   ```bash
   dotnet run --project MagazineAPI
   ```

5. Aplikacja powinna być dostępna pod adresem `http://localhost:5000`.
