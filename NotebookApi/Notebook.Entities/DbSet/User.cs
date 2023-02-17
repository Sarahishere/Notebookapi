namespace Notebook.Entities.DbSet;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNum { get; set; }
    public string DateOfBirth { get; set; }
    public string Country { get; set; }
}