using System.ComponentModel.DataAnnotations;

namespace Notebook.Auth.Models.Dtos.IncomingDto;

public class RegisterRequestDto
{
    [Required] 
    public string FirstName { get; set; }
    [Required] 
    public string LastName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
}