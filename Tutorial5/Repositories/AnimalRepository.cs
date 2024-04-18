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
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area);";
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", (object)animal.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Category", (object)animal.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("@Area", (object)animal.Area ?? DBNull.Value);
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
                return new Animal()
                {
                    IdAnimal = reader.GetInt32(reader.GetOrdinal("IdAnimal")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                    Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? null : reader.GetString(reader.GetOrdinal("Area"))
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
        command.CommandText = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal;";
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", (object)animal.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Category", (object)animal.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("@Area", (object)animal.Area ?? DBNull.Value);
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