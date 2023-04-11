using Microsoft.AspNetCore.Mvc;

namespace JsonTransformerApi.Controllers
{
    /// <summary>
    /// Controller for token management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ITokenProvider tokenProvider, ILogger<TokenController> logger)
        {
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

        /// <summary>
        /// Gets a token for the specified password.
        /// </summary>
        /// <param name="password">The password to use.</param>
        /// <returns>The token, if successful; otherwise, a bad request (400) status code.</returns>
        [HttpPost()]
        public IActionResult GetToken([FromBody] string password)
        {
            try
            {
                var (isSuccess, token) = _tokenProvider.GetToken(password);

                return isSuccess ? Ok(token) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(500); //Internal Server Error
            }

        }
    }
}
