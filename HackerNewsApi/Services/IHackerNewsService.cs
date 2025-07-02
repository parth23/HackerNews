using HackerNewsApi.Models;

namespace HackerNewsApi.Services
{
    public interface IHackerNewsService
    {
        Task<PaginatedResult<HackerNewsStory>> GetNewestStoriesAsync(StoryQueryParameters parameters);
        Task<List<HackerNewsStory>> GetAllNewestStoriesAsync();
    }
}
