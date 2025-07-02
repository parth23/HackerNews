using HackerNewsApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace HackerNewsApi.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetNewestStories_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/stories/newest");

            // Assert
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaginatedResult<HackerNewsStory>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.True(result.Items.Count > 0);
            Assert.True(result.TotalCount > 0);
        }

        [Fact]
        public async Task GetNewestStories_WithPagination_ReturnsCorrectPage()
        {
            // Act
            var response = await _client.GetAsync("/api/stories/newest?page=1&pageSize=5");

            // Assert
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaginatedResult<HackerNewsStory>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(5, result.PageSize);
            Assert.True(result.Items.Count <= 5);
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsHealthy()
        {
            // Act
            var response = await _client.GetAsync("/api/stories/health");

            // Assert
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("healthy", content);
        }

        [Fact]
        public async Task GetNewestStories_InvalidPage_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/stories/newest?page=0");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
