using APBD4.Models;
using APBD4.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private SqlDbService sqlDbService;
        public AnimalsController(SqlDbService sqlDbService) {
            this.sqlDbService = sqlDbService;
        }
        

        //Select * animals ordered by orderBy
        [HttpGet]
        public IActionResult GetAnimals([FromQuery]string? orderBy = null)
        {
            string result = sqlDbService.AnimalListToJSON(sqlDbService.GetAnimalsOrderedBy(orderBy));
            if (result.Equals("") || result == null)
            {
                return BadRequest("Invalid sorting category entered!");
            }
            else
            {
                return Ok(result);
            }
        }

        //accepts data in JSON format
        [HttpPost]
        public IActionResult AddAnimal(Animal newAnimal)
        {
            if (newAnimal == null)
            {
                return BadRequest("New animal cannot be null!");
            }
            //if successful
            if (sqlDbService.AddAnimal(newAnimal))
            {
                return Ok("Successfully added new animal: " + newAnimal.Name);
            }
            else
            {
                return BadRequest("New Animal data is incomplete!");
            }
        }

        //accepts JSON, primary key cannot be modified
        [HttpPut("{idAnimal}")]
        public IActionResult UpdateAnimal(int idAnimal, Animal updatedAnimal)
        {
            if(idAnimal != updatedAnimal.IdAnimal)
            {
                return BadRequest("idAnimal mismatch");
            }
            if(sqlDbService.UpdateAnimal(idAnimal, updatedAnimal))
            {
                return Ok("Successfully updated " + updatedAnimal.Name + " data");
            }
            else
            {
                return BadRequest("Animal with id: " + idAnimal + " doesn't exist in the database");
            }
            
        }

        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal(int idAnimal)
        {
            if(sqlDbService.DeleteAnimal(idAnimal))
            {
                return Ok("Deleted animal with id: " + idAnimal);
            }
            else
            {
                return BadRequest("Animal with requested id does not exist in database");
            }
        }
    }
}
