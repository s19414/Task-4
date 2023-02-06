using APBD4.Models;
using APBD4.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        SqlDbService sqlDbService = new SqlDbService();

        [HttpGet]
        public IActionResult GetAnimals([FromQuery]string orderBy)
        {
            
            return Ok(sqlDbService.getConString());
        }

        //accepts data in JSON format
        [HttpPost]
        public IActionResult AddAnimal(Animal newAnimal)
        {
            return null;
        }

        //accepts JSON, primary key cannot be modified
        [HttpPut("{idAnimal}")]
        public IActionResult UpdateAnimal(int idAnimal)
        {
            return null;
        }

        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal(int idAnimal)
        {
            return null;
        }
    }
}
