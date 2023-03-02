using System.ComponentModel.DataAnnotations;

namespace Notebook.Auth.Models.Dtos.IncomingDto;

public class LoginRequestDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    
}