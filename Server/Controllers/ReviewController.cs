using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using Core.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MovieServer.Hubs;

namespace MovieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IGenericRepository<Review> _repository;
        private readonly IHubContext<ReviewHub> _hubContext;
        public ReviewController(IGenericRepository<Review> repository, IHubContext<ReviewHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreateReviw(ReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var review = new Review()
                {
                    Content = reviewDto.Content,
                    MovieId = reviewDto.MovieId,
                    UserId = reviewDto.UserId
                };

                await _repository.Create(review);
                await _hubContext.Clients.Group(reviewDto.MovieId.ToString()).SendAsync("Receivereview", review);

                return Ok("Created successfully");
            }
            catch
            {
                return BadRequest("can't create the review");
            }

        }
        [HttpGet]
        public async Task<IEnumerable<Review>> GetAllReview()
        {
            return await _repository.GetAllAsync();
        }
    }
}
