using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DictionaryWebSite.Controllers;
using DictionaryWebSite.Models_DTOs.Security;
using Xunit;

namespace DictionaryWebsite.Tests
{
    public class AuthControllerTests
    {
        private AuthController CreateController()
        {
            // Build a fake configuration just like appsettings.json would provide
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "TestSecretKeyThatIsAtLeast32CharsLong!" },
                { "Jwt:Issuer", "DictionaryApp" },
                { "Jwt:Audience", "DictionaryAppUsers" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new AuthController(config);
        }

        [Fact]
        public void Login_WithCorrectCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var controller = CreateController();
            var request = new LoginRequest
            {
                Username = "moderator",
                Password = "Calafia33"
            };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);

            Assert.False(string.IsNullOrEmpty(response.Token));
            Assert.Equal("moderator", response.Username);
            Assert.Equal("Moderator", response.Role);
        }

        [Fact]
        public void Login_WithWrongPassword_ReturnsUnauthorized()
        {
            // Arrange
            var controller = CreateController();
            var request = new LoginRequest
            {
                Username = "moderator",
                Password = "WrongPassword"
            };

            // Act
            var result = controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_WithWrongUsername_ReturnsUnauthorized()
        {
            // Arrange
            var controller = CreateController();
            var request = new LoginRequest
            {
                Username = "notamoderator",
                Password = "YourPassword123!"
            };

            // Act
            var result = controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_WithEmptyCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var controller = CreateController();
            var request = new LoginRequest
            {
                Username = "",
                Password = ""
            };

            // Act
            var result = controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
