namespace Notebook.Entities.Dtos.IncomingDto;

public class UserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNum { get; set; }
    public string DateOfBirth { get; set; } //todo: need to change back to DateTime for official use
    public string Country { get; set; }
}