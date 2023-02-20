using Microsoft.AspNetCore.Mvc;
using Notebook.DataService.Data;
using Notebook.Entities.DbSet;
using Notebook.Entities.Dtos.IncomingDto;

namespace Notebook.Endpoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public UsersController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //Post 
    [HttpPost]
    public IActionResult CreateUser(UserDto user)
    {
        var newUser = new User();
        newUser.Country = user.Country;
        newUser.Email = user.Email;
        newUser.FirstName = user.Email;
        newUser.LastName = user.LastName;
        newUser.PhoneNum = user.LastName;
        newUser.DateOfBirth = DateTime.Parse(user.DateOfBirth);
        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        return Ok();//todo: return 201
    }
    // Get all users
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _dbContext.Users.Where(x => x.Status == 1).ToList();
        return Ok(users);
    }
    // Get user by id
    [HttpGet]
    [Route("getById")]
    public IActionResult GetUserById(Guid id)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
}