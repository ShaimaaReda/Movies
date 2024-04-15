using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using Core.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MovieServer.Hubs;
using System.IO;
using System.Net.WebSockets;

namespace MovieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IGenericRepository<Movie> _repository;

        public MovieController(IGenericRepository<Movie> repository)
        {
            _repository = repository;
           
        }

        /// <summary>
        /// //////////////////////////////////////////


        // POST: api/Movie
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromForm] MovieDto movieDto, IFormFile poster)
        {
            try
            {
                if (poster == null || poster.Length == 0)
                return BadRequest("Poster is required.");

                var movie = new Movie();
            using (var memoryStream = new MemoryStream())
            {
                await poster.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();


                movie = new Movie
                {
                    Name = movieDto.Name,
                    Details = new MovieDetail
                    {
                        Description = movieDto.Description,
                        Actor = movieDto.Actor,
                        Poster = imageBytes
                    }
                };
            }
                _repository.Create(movie);

                return Ok("add movie succesfully");
        }
             catch (Exception ex)
    {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        

        [HttpGet]
        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await _repository.GetAllAsync();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMovie([FromBody]Movie movie)
        {
            if (await _repository.GetByIdAsync(movie.Id)!=null)
            {
                _repository.UpdateAsync(movie);
                return Ok();
            }

            else 
                return BadRequest("this movie does't exist");
        }

    }
}
