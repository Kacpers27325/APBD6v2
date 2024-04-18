using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    private IAnimalRepository _animalRepositoryImplementation;

    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy};";

        var reader = command.ExecuteReader();
            
        var animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(reader.GetOrdinal("IdAnimal")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? null : reader.GetString(reader.GetOrdinal("Area"))
            });
        }

        return animals;
    }

    public void AddAnimal(Animal animal)
    {
        // Otwieramy połaczenie do bazy danych
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        // Definiujemy query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName, '', '', '');";
        command.Parameters.AddWithValue("animalName", animal.Name);

        command.ExecuteNonQuery();
    }
    
    public Animal GetAnimalById(int idAnimal)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            connection.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Animal WHERE IdAnimal = @idAnimal;";
            command.Parameters.AddWithValue("idAnimal", idAnimal);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
                int nameOrdinal = reader.GetOrdinal("Name");

                return new Animal()
                {
                    IdAnimal = reader.GetInt32(idAnimalOrdinal),
                    Name = reader.GetString(nameOrdinal)
                    // Dodaj resztę pól Animal, jeśli są dostępne w bazie danych
                };
            }

            return null;
        }

        public void UpdateAnimal(int idAnimal, Animal animal)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            connection.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE Animal SET Name = @animalName WHERE IdAnimal = @idAnimal;";
            command.Parameters.AddWithValue("idAnimal", idAnimal);
            command.Parameters.AddWithValue("animalName", animal.Name);

            command.ExecuteNonQuery();
        }

        public void DeleteAnimal(int idAnimal)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            connection.Open();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM Animal WHERE IdAnimal = @idAnimal;";
            command.Parameters.AddWithValue("idAnimal", idAnimal);
            command.ExecuteNonQuery();
        }
}