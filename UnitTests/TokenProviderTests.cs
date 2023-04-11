using JsonTransformerApi.Options;
using JsonTransformerApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UnitTests
{
    [TestFixture]
    public class TokenProviderTests
    {
        private Mock<IOptions<JwtOptions>> _optionsMock;
        private Mock<ILogger<ITokenProvider>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _optionsMock = new Mock<IOptions<JwtOptions>>();
            _loggerMock = new Mock<ILogger<ITokenProvider>>();
        }

        [Test]
        public void GetToken_WithValidPassword_ReturnsToken()
        {
            // Arrange
            var jwtOptions = new JwtOptions
            {
                Issuer = "TestIssuer",
                Key = "LongKeyForTestPurposes",
                TtlMinutes = 30,
                AllowedPasswords = new HashSet<string> { "TestPassword" }
            };
            _optionsMock.Setup(x => x.Value).Returns(jwtOptions);

            var tokenProvider = new TokenProvider(_optionsMock.Object, _loggerMock.Object);

            // Act
            var result = tokenProvider.GetToken("TestPassword");

            // Assert
            result.isSuccess.Should().BeTrue();
            result.token.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetToken_WithInvalidPassword_ReturnsIsSuccessFalse()
        {
            // Arrange
            var jwtOptions = new JwtOptions
            {
                Issuer = "TestIssuer",
                Key = "LongKeyForTestPurposes",
                TtlMinutes = 30,
                AllowedPasswords = new HashSet<string> { "TestPassword" }
            };
            _optionsMock.Setup(x => x.Value).Returns(jwtOptions);

            var tokenProvider = new TokenProvider(_optionsMock.Object, _loggerMock.Object);

            // Act
            var result = tokenProvider.GetToken("InvalidPassword");

            // Assert
            result.isSuccess.Should().BeFalse();
            result.token.Should().BeEmpty();
        }
    }

}
