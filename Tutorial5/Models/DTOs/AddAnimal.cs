using System.ComponentModel.DataAnnotations;

namespace Tutorial5.Models.DTOs;

public class AddAnimal
{
    [Required]
    [MinLength(5)]
    public string Name { get; set; }

    public string? Description { get; set; }
}