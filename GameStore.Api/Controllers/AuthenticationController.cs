using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Configurations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly JwtConfig _jwtConfig;

        public AuthenticationController(UserManager<IdentityUser> userManager, JwtConfig jwtConfig)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig;

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

    }
}