using Microsoft.AspNetCore.Mvc;
using Notebook.DataService.Data;
using Notebook.DataService.IConfiguration;
using Notebook.Entities.DbSet;
using Notebook.Entities.Dtos.IncomingDto;

namespace Notebook.Endpoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    //Post 
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDto user)
    {
        var newUser = new User();
        newUser.Country = user.Country;
        newUser.Email = user.Email;
        newUser.FirstName = user.FirstName;
        newUser.LastName = user.LastName;
        newUser.PhoneNum = user.PhoneNum;
        newUser.DateOfBirth = DateTime.Parse(user.DateOfBirth);
        await _unitOfWork.Users.Create(newUser);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction("GetUserById",newUser.Id,newUser);
    }
    // Get all users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _unitOfWork.Users.GetAll();
        return Ok(users);
    }
    // Get user by id
    [HttpGet]
    [Route("getById")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _unitOfWork.Users.GetById(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
}