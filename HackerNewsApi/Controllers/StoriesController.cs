using HackerNewsApi.Models;
using HackerNewsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly ILogger<StoriesController> _logger;

        public StoriesController(IHackerNewsService hackerNewsService, ILogger<StoriesController> logger)
        {
            _hackerNewsService = hackerNewsService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated newest stories from Hacker News
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 20, max: 100)</param>
        /// <param name="search">Search term to filter stories by title</param>
        /// <returns>Paginated list of stories</returns>
        [HttpGet("newest")]
        public async Task<ActionResult<PaginatedResult<HackerNewsStory>>> GetNewestStories(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null)
        {
            try
            {
                // Validate parameters
                if (page < 1)
                {
                    return BadRequest("Page number must be greater than 0");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100");
                }

                var parameters = new StoryQueryParameters
                {
                    Page = page,
                    PageSize = pageSize,
                    Search = search?.Trim()
                };

                var result = await _hackerNewsService.GetNewestStoriesAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting newest stories");
                return StatusCode(500, "An error occurred while fetching stories");
            }
        }

        /// <summary>
        /// Get health check endpoint
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}
