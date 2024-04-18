using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;
using Tutorial5.Repositories;

namespace Tutorial5.Controllers;
[ApiController]
// [Route("/api/animals")]
[Route("/api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    public AnimalsController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "Name") {
        var animals = _animalRepository.GetAnimals(orderBy);
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        _animalRepository.AddAnimal(animal);
        return Created("/api/animals", null);
    }
    
    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, Animal animal)
    {
        var existingAnimal = _animalRepository.GetAnimalById(idAnimal);
        if (existingAnimal == null)
        {
            return NotFound();
        }
        _animalRepository.UpdateAnimal(idAnimal, animal);
        return NoContent();
    }
    
    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        _animalRepository.DeleteAnimal(idAnimal);
        return NoContent();
    }
}