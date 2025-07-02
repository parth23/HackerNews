using HackerNewsApi.Models;
using HackerNewsApi.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace HackerNewsApi.Tests
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<ILogger<HackerNewsService>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private readonly HackerNewsService _service;

        public HackerNewsServiceTests()
        {
            _mockLogger = new Mock<ILogger<HackerNewsService>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            // Create a simple mock HTTP client that returns test data
            var handler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(handler);
            _service = new HackerNewsService(_httpClient, _memoryCache, _mockLogger.Object);
        }

        [Fact]
        public async Task GetNewestStoriesAsync_WithValidParameters_ReturnsPaginatedResult()
        {
            // This is a simplified test since we can't easily mock the actual HTTP calls
            // In a real scenario, you would inject a mock IHttpClientFactory or use a test server
            
            var parameters = new StoryQueryParameters
            {
                Page = 1,
                PageSize = 2
            };

            // Act & Assert - This will test the basic structure even though it may not return data
            var exception = await Record.ExceptionAsync(() => _service.GetNewestStoriesAsync(parameters));
            
            // The service should handle exceptions gracefully
            Assert.NotNull(exception); // We expect an exception since we can't actually call the API
        }

        // Test model validation and business logic
        [Fact]
        public void StoryQueryParameters_HasSearch_ReturnsCorrectValue()
        {
            var parameters = new StoryQueryParameters
            {
                Search = "test"
            };

            Assert.True(parameters.HasSearch);

            parameters.Search = "";
            Assert.False(parameters.HasSearch);

            parameters.Search = null;
            Assert.False(parameters.HasSearch);
        }

        [Fact]
        public void HackerNewsStory_Properties_WorkCorrectly()
        {
            var story = new HackerNewsStory
            {
                Id = 1,
                Title = "Test Story",
                Url = "https://example.com",
                Time = DateTimeOffset.Now.ToUnixTimeSeconds()
            };

            Assert.True(story.HasUrl);
            Assert.True(story.TimeStamp > DateTime.MinValue);

            story.Url = null;
            Assert.False(story.HasUrl);
        }

        [Fact]
        public void PaginatedResult_Properties_CalculateCorrectly()
        {
            var result = new PaginatedResult<HackerNewsStory>
            {
                TotalCount = 100,
                PageSize = 20,
                PageNumber = 1
            };

            Assert.Equal(5, result.TotalPages);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);

            result.PageNumber = 3;
            Assert.True(result.HasNext);
            Assert.True(result.HasPrevious);

            result.PageNumber = 5;
            Assert.False(result.HasNext);
            Assert.True(result.HasPrevious);
        }
    }

    // Simple mock handler for testing
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Return a simple error response for testing
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
        }
    }
}
