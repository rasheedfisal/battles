using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Requests;

public class UpsertDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
