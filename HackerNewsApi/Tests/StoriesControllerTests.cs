using HackerNewsApi.Controllers;
using HackerNewsApi.Models;
using HackerNewsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HackerNewsApi.Tests
{
    public class StoriesControllerTests
    {
        private readonly Mock<IHackerNewsService> _mockService;
        private readonly Mock<ILogger<StoriesController>> _mockLogger;
        private readonly StoriesController _controller;

        public StoriesControllerTests()
        {
            _mockService = new Mock<IHackerNewsService>();
            _mockLogger = new Mock<ILogger<StoriesController>>();
            _controller = new StoriesController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetNewestStories_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var expectedResult = new PaginatedResult<HackerNewsStory>
            {
                Items = new List<HackerNewsStory>
                {
                    new HackerNewsStory
                    {
                        Id = 1,
                        Title = "Test Story",
                        Url = "https://example.com",
                        By = "testuser",
                        Time = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        Score = 100,
                        Type = "story"
                    }
                },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 20
            };

            _mockService
                .Setup(s => s.GetNewestStoriesAsync(It.IsAny<StoryQueryParameters>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetNewestStories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsType<PaginatedResult<HackerNewsStory>>(okResult.Value);
            Assert.Equal(expectedResult.Items.Count, actualResult.Items.Count);
            Assert.Equal(expectedResult.TotalCount, actualResult.TotalCount);
        }

        [Fact]
        public async Task GetNewestStories_WithInvalidPageNumber_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetNewestStories(page: 0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Page number must be greater than 0", badRequestResult.Value);
        }

        [Fact]
        public async Task GetNewestStories_WithInvalidPageSize_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetNewestStories(pageSize: 0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Page size must be between 1 and 100", badRequestResult.Value);
        }

        [Fact]
        public async Task GetNewestStories_WithPageSizeTooLarge_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetNewestStories(pageSize: 101);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Page size must be between 1 and 100", badRequestResult.Value);
        }

        [Fact]
        public async Task GetNewestStories_WithServiceException_ReturnsInternalServerError()
        {
            // Arrange
            _mockService
                .Setup(s => s.GetNewestStoriesAsync(It.IsAny<StoryQueryParameters>()))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetNewestStories();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public void Health_ReturnsOkResult()
        {
            // Act
            var result = _controller.Health();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
