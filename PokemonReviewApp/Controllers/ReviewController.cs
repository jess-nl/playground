using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;

        public ReviewController(IReviewRepository reviewRepository, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();

            var reviewsDto = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Title = r.Title,
                Text = r.Text,
                Rating = r.Rating
            }).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewsDto);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                Title = review.Title,
                Text = review.Text,
                Rating = review.Rating
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewDto);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews = _reviewRepository.GetReviewsOfPokemon(pokeId);

            var reviewsDto = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Title = r.Title,
                Text = r.Text,
                Rating = r.Rating
            }).ToList();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviewsDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToLower() == reviewCreate.Title.TrimEnd().ToLower())
                .FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = new Review
            {
                Title = reviewCreate.Title,
                Text = reviewCreate.Text,
                Rating = reviewCreate.Rating,
            };

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = new Review
            {
                Id = updatedReview.Id,
                Title = updatedReview.Title,
                Text = updatedReview.Text,
                Rating = updatedReview.Rating,
            };

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}