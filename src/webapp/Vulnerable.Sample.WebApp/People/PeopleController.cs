using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Vulnerable.Sample.WebApp.People 
{
    [Route("/api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService) => 
            _peopleService = peopleService;

        [HttpGet]
        public IActionResult Get([FromQuery] string name)
        {
            var people = _peopleService.FindPeopleByName(name);

            if(people == null || !people.Any())
                return NoContent();

            return Ok(people);
        }
    }
}