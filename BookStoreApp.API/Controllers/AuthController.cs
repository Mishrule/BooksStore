using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager)
    {
      _logger = logger;
      _mapper = mapper;
      _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
      _logger.LogInformation($"Registration Attempt for {userDto.Email}");
      try
      {
        var user = _mapper.Map<ApiUser>(userDto);
        user.UserName = userDto.Email;
        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (result.Succeeded == false)
        {
          foreach (var error in result.Errors)
          {
            ModelState.AddModelError(string.Empty, error.Description);
          }

          return BadRequest(ModelState);
        }

        await _userManager.AddToRoleAsync(user, "User");
        //if (string.IsNullOrEmpty(userDto.Role))
        //{
        //  await _userManager.AddToRoleAsync(user, "User");
        //}
        //else
        //{
        //  await _userManager.AddToRoleAsync(user, userDto.Role);
        //}
        return Accepted();
      }
      catch (Exception ex)
      {
        _logger.LogError($"Something went wrong in the {nameof(Register)} method: {ex.Message} statusCode:{500}");
        return Problem($"Something went wrong in the {nameof(Register)} method: {ex.Message}", statusCode: 500);
      }
     
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDto userDto)
    {
      _logger.LogInformation($"Login Attempt for {userDto.Email}");
      try
      {
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);
        if (user == null || passwordValid == false)
        {
          return NotFound();
        }

        
        //var token = await _userManager.GenerateUserTokenAsync(user, "Auth", null);
        //return Ok(new { token });
        return Accepted();
      }
      catch (Exception ex)
      {
        _logger.LogError($"Something went wrong in the {nameof(Login)} method: {ex.Message} statusCode:{500}");
        return Problem($"Something went wrong in the {nameof(Login)} method: {ex.Message}", statusCode: 500);
      }
    }
  }
}
