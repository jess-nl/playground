# PlayGround

C#/.NET projects to sharpen my skills, revise, practice and learn. 
<br>
*Projects presented thus far:*

1. PokemonReviewApp

    > *Web API, MVC, CRUD*

    - Based on the following YouTube tutorial, [ASP.NET Core Web API .NET 6](https://www.youtube.com/watch?v=_8nLSsK5NDo&list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0)

    What I did differently from tutorial:
   - Employ *.NET 8* instead of *.NET 6*
   - Avoid exposing connection string
   - Implement object mapping instead of *AutoMapper*
   - Ensure all `BadRequest()` passes in `ModelState` argument
   - Edit logic for `GetPokemonByOwner()` as it should return a list, not a single object