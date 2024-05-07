using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameStore.Api.Configurations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace GameStore.Api.Controllers
{

    // Controller for handling user authentication requests in an ASP.NET Core API,
    // using UserManager<IdentityUser> for user management and JwtConfig for JWT configuration.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly IConfiguration _configuration;
        //private readonly JwtConfig _jwtConfig;

        public AuthenticationController(UserManager<IdentityUser> userManager,
        IConfiguration configuration
         //JwtConfig jwtConfig
         )
        {
            _userManager = userManager;
            // _jwtConfig = jwtConfig;
            _configuration = configuration;

        }

        // Registers a new user by checking if the provided email is not already in use.
        // If the email is unique, creates a new user with the provided email and password.
        // Returns a BadRequest response if the email is already in use or if there is a server error.
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            //validate the incoming request
            if (ModelState.IsValid)
            {
                //we need to check if the email already exist
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);

                if (user_exist != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "email already exist"

                        }

                    });
                }

                //create a user
                var new_user = new IdentityUser()
                {
                    //data which we receive from user
                    Email = requestDto.Email,
                    UserName = requestDto.Email
                };

                var is_created = await _userManager.CreateAsync(new_user, requestDto.Password);

                //check if it was created succesfully
                if (is_created.Succeeded)
                {
                    //generate the token
                    var token = GenerateJwtToken(new_user);

                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    });
                }

                //if is_created is not succeded, we're gonna return an error
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>(){
                        "Server error"
                    },
                    Result = false

                });
            }

            return BadRequest();

        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            // Start date: Now; when the timer starts, to make token expire
            // Set the notBefore date to the current date and time.
            var notBefore = DateTime.Now;

            // Expiration date: Now + 1 hour
            // Set the expiration date to one hour after the current date and time.
            var expires = DateTime.Now.AddHours(1);

            //token descriptor: to put all the configurations we need inside the token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = expires,
                NotBefore = notBefore,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;

        }

    }
}