using Tutorial5.Models;
using Tutorial5.Models.DTOs;


namespace Tutorial5.Repositories
{
    public interface IAnimalRepository
    {
        IEnumerable<Animal> GetAnimals(string orderBy);
        void AddAnimal(Animal animal);
        Animal GetAnimalById(int idAnimal);
        void UpdateAnimal(int idAnimal, Animal animal);
        void DeleteAnimal(int idAnimal);
    }
}