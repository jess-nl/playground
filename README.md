# PlayGround

C#/.NET projects to sharpen my skills, revise, practice and learn. 
<br>
*Projects presented thus far:*

1. PokemonReviewApp

    > *Web API, MVC, CRUD*

    - Based on the following YouTube tutorial playlist, [ASP.NET Core Web API .NET 6](https://www.youtube.com/watch?v=_8nLSsK5NDo&list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0)

    What I did differently from tutorial:
   - Employ *.NET 8* instead of *.NET 6*
   - Avoid exposing connection string
   - Implement object mapping instead of *AutoMapper*
   - Ensure all `BadRequest()` passes in `ModelState` argument
   - Edit logic for `GetPokemonByOwner()` as it should return a list, not a single object

2. DotNet8IdentityAuthentication
    > *Authentication, authorization, Identity of Entity Framework*
    
    - Based on the following YouTube tutorial, [.NET 8 Authentication with Identity in a Web API with Bearer Tokens & Cookies](https://www.youtube.com/watch?v=8J3nuUegtL4)

3. RoleBasedJWT
    > *Authentication, role-based authorization, JWT*

    - Based on the following two YouTube tutorials:
        - [.NET 7 Web API 🔒 Create JSON Web Tokens (JWT) - User Registration / Login / Authentication
](https://www.youtube.com/watch?v=UwruwHl3BlU)
        - [.NET 7 Web API 🔒 Role-Based Authorization with JSON Web Tokens (JWT) & the dotnet user-jwts CLI
](https://www.youtube.com/watch?v=6sMPvucWNRE)

    What I did differently from tutorial:
    - Employ .NET 8 instead of .NET 7