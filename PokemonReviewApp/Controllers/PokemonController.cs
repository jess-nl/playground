using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository)

        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();

            var pokemonsDto = pokemons.Select(p => new PokemonDto
            {
                Id = p.Id,
                Name = p.Name,
                BirthDate = p.BirthDate
            }).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonsDto);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _pokemonRepository.GetPokemon(pokeId);

            var pokemonDto = new PokemonDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonDto);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemonFound = _pokemonRepository.GetPokemons()
                .Where(c => c.Name.Trim().ToLower() == pokemonCreate.Name.TrimEnd().ToLower())
                .FirstOrDefault();

            if (pokemonFound != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = new Pokemon
            {
                Name = pokemonCreate.Name,
                BirthDate = pokemonCreate.BirthDate
            };

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(
            int pokemonId,
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDto updatedPokemon
            )
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = new Pokemon
            {
                Id = updatedPokemon.Id,
                Name = updatedPokemon.Name,
                BirthDate = updatedPokemon.BirthDate,
            };

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewsOfPokemon(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                ModelState.AddModelError("", "Something went wrong while deleting reviews");

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
                ModelState.AddModelError("", "Something went wrong while deleting pokemon");

            return NoContent();
        }
    }
}
