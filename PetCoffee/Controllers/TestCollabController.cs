using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PetCoffee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCollabController : ApiControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
