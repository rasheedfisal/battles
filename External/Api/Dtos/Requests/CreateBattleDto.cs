using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Requests;

public class CreateBattleDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
