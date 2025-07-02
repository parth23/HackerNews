using HackerNewsApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNewsApi.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HackerNewsService> _logger;
        private const string CACHE_KEY = "newest_stories";
        private const string BASE_URL = "https://hacker-news.firebaseio.com/v0";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache, ILogger<HackerNewsService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<PaginatedResult<HackerNewsStory>> GetNewestStoriesAsync(StoryQueryParameters parameters)
        {
            try
            {
                var allStories = await GetAllNewestStoriesAsync();
                
                // Apply search filter if provided
                var filteredStories = allStories;
                if (parameters.HasSearch && !string.IsNullOrEmpty(parameters.Search))
                {
                    filteredStories = allStories
                        .Where(s => s.Title?.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) == true)
                        .ToList();
                }

                // Apply pagination
                var totalCount = filteredStories.Count;
                var pagedStories = filteredStories
                    .Skip((parameters.Page - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .ToList();

                return new PaginatedResult<HackerNewsStory>
                {
                    Items = pagedStories,
                    TotalCount = totalCount,
                    PageNumber = parameters.Page,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching newest stories");
                throw;
            }
        }

        public async Task<List<HackerNewsStory>> GetAllNewestStoriesAsync()
        {
            // Check cache first
            if (_cache.TryGetValue(CACHE_KEY, out List<HackerNewsStory>? cachedStories) && cachedStories != null)
            {
                _logger.LogInformation("Returning cached stories");
                return cachedStories;
            }

            try
            {
                _logger.LogInformation("Fetching newest stories from HackerNews API");
                
                // Get the list of newest story IDs
                var newStoriesUrl = $"{BASE_URL}/newstories.json";
                var storyIdsResponse = await _httpClient.GetStringAsync(newStoriesUrl);
                var storyIds = JsonSerializer.Deserialize<int[]>(storyIdsResponse) ?? Array.Empty<int>();

                // Fetch details for the first 500 stories (to have enough for pagination)
                var storyTasks = storyIds.Take(500).Select(GetStoryDetailsAsync);
                var stories = await Task.WhenAll(storyTasks);

                // Filter out null stories and sort by time (newest first)
                var validStories = stories
                    .Where(s => s != null)
                    .Cast<HackerNewsStory>()
                    .OrderByDescending(s => s.Time)
                    .ToList();

                // Cache the results
                _cache.Set(CACHE_KEY, validStories, _cacheExpiration);
                
                _logger.LogInformation($"Successfully fetched {validStories.Count} stories");
                return validStories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stories from HackerNews API");
                throw;
            }
        }

        private async Task<HackerNewsStory?> GetStoryDetailsAsync(int storyId)
        {
            try
            {
                var storyUrl = $"{BASE_URL}/item/{storyId}.json";
                var storyResponse = await _httpClient.GetStringAsync(storyUrl);
                var story = JsonSerializer.Deserialize<HackerNewsStory>(storyResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return story;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to fetch story {storyId}: {ex.Message}");
                return null;
            }
        }
    }
}
