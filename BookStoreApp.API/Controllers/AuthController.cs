﻿using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
 // [AllowAnonymous]
  public class AuthController : ControllerBase
  {
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
      _logger = logger;
      _mapper = mapper;
      _userManager = userManager;
      _configuration = configuration;
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
    public async Task<ActionResult<AuthResponse>> Login(LoginDto userDto)
    {
      _logger.LogInformation($"Login Attempt for {userDto.Email}");
      try
      {
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);
        if (user == null || passwordValid == false)
        {
          return Unauthorized(userDto);
        }

        
        string tokenString = await GenerateToken(user);

        var response = new AuthResponse
        {
          Email = userDto.Email,
          Token = tokenString,
          UserId = user.Id
        };

        //return Ok(new { token });
       //or return Accepted(response);
        //or OK(response);
        return response;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Something went wrong in the {nameof(Login)} method: {ex.Message} statusCode:{500}");
        return Problem($"Something went wrong in the {nameof(Login)} method: {ex.Message}", statusCode: 500);
      }
    }

    private async Task<string> GenerateToken(ApiUser user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var roles = await _userManager.GetRolesAsync(user);
      var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

      //Claims from Database
      var userClaims = await _userManager.GetClaimsAsync(user);

      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(CustomClaimTypes.uid, user.Id)
      }
      .Union(userClaims)
      .Union(roleClaims);


      var token = new JwtSecurityToken(
          issuer: _configuration["JwtSettings:Issuer"],
          audience: _configuration["JwtSettings:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
          signingCredentials: credentials
        );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
