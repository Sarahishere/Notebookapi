using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notebook.Auth.Models.Dtos.IncomingDto;
using Notebook.Auth.Models.Dtos.OutgoingDto;
using Notebook.DataService.IConfiguration;
using Notebook.Endpoint.Configuration.Models;
using Notebook.Entities.DbSet;

namespace Notebook.Endpoint.Controllers.V1;

public class AccountsController : BaseController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JWTConfig _jwtConfig;
    
    public AccountsController(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        IOptionsMonitor<JWTConfig> optionsMonitor) : base(unitOfWork)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        //validate received obj
        if (!ModelState.IsValid)
        {
            return BadRequest(new RegisterResponseDto
            {
                IsSuccess = false,
                Errors = new List<string>
                {
                    "Invalid payload"
                }
                    
            });
        }
        //check if the identity user already exist
        var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);
        if (existingUser != null)
        {
            return BadRequest(new RegisterResponseDto
            {
                IsSuccess = false,
                Errors = new List<string>
                {
                    "User has already been registered before"
                }
                    
            });
        }
        //add IdentiyUser and user
        var newIdentityUser = new IdentityUser
        {
            Email = registerRequestDto.Email,
            UserName = registerRequestDto.Email,
            EmailConfirmed = true //TODO: implement functionality to confirm email
        };
        var isCreated = _userManager.CreateAsync(newIdentityUser, registerRequestDto.Password);
        
        if (!isCreated.Result.Succeeded)
        {
            return BadRequest(new RegisterResponseDto
            {
                IsSuccess = false,
                Errors = new List<string>
                {
                    "User register failed",
                    $"{isCreated.Result.ToString()}"
                    
                }
                    
            });
        }

        var newUser = new User
        {
            IdentityId = new Guid(newIdentityUser.Id),
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName,
            Email = registerRequestDto.Email,
            PhoneNum = "",
            Country = ""
        };
        await _unitOfWork.Users.Create(newUser);
        await _unitOfWork.CompleteAsync(); 
        
        //generate token
        var token = GenerateToken(newIdentityUser);
        //return token
        return Ok(new RegisterResponseDto
        {
            Token = token,
            IsSuccess = true
        });
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new LoginResponseDto
            {
                IsSuccess = false,
                Errors = new List<string>
                {
                    "Invalid payload"
                }
            });
        }
        var existingUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
        if (existingUser == null)
        {
            return BadRequest(new LoginResponseDto
            {
                IsSuccess = false,
                Errors = new List<string>
                {
                    "Invalid Email"
                }
            });           
        }

        var isCorrectPassword = await _userManager.CheckPasswordAsync(existingUser, loginRequestDto.Password);
        if (!isCorrectPassword)
        {
            return BadRequest(new LoginResponseDto
            {
                IsSuccess = false,
                Errors = new List<string>
                {
                    "Invalid Password"
                }
            });                  
        }

        var token = GenerateToken(existingUser);
        return Ok(new LoginResponseDto
        {
            Token = token,
            IsSuccess = true
        });
    }

    private string GenerateToken(IdentityUser user)
    {
        //create handler
        var jwtHandler = new JwtSecurityTokenHandler();
        //get key
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        //descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256),
            Expires = DateTime.Now.AddHours(3) // TODO: update to minutes
        };

        var token = jwtHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtHandler.WriteToken(token);
        return jwtToken;
    }
}