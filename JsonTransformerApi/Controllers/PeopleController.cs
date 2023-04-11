using JsonTransformerApi.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JsonTransformerApi.Controllers
{
    /// <summary>
    /// Controller for people management
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPeopleManager _manager;

        public PeopleController(IPeopleManager manager, ILogger<PeopleController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        /// <summary>
        /// Transforms an array of flat persons into a hierarchical structure
        /// </summary>
        [HttpPost("transform")]
        public async Task<IActionResult> Post(Person[] persons)
        {
            if (persons == null || persons.Length == 0)
                return Ok(new List<PersonTreeNode>());

            try
            {
                var result = await _manager.ProcessPeople(persons);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(500); //Internal Server Error
            }
            
        }
    }
}