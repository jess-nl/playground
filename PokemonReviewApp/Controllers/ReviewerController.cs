using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewerController(IReviewerRepository reviewerRepository)
        {
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();

            var reviewersDto = reviewers.Select(r => new ReviewerDto
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
            }).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewersDto);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);

            var reviewerDto = new ReviewerDto
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName,
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewerDto);
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var reviewerFound = _reviewerRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToLower() == reviewerCreate.LastName.TrimEnd().ToLower())
                .FirstOrDefault();

            if (reviewerFound != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = new Reviewer
            {
                FirstName = reviewerCreate.FirstName,
                LastName = reviewerCreate.LastName,
            };

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = new Reviewer
            {
                Id = updatedReviewer.Id,
                FirstName = updatedReviewer.FirstName,
                LastName = updatedReviewer.LastName,
            };

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
