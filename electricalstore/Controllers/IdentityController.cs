using System;
using System.Threading.Tasks;
using Contracts.Repositories;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace electricalstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityRepositoryWrapper _repository;

        public IdentityController(IIdentityRepositoryWrapper repository)
        {
            _repository = repository;
        }

        //Post api/registration
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegiterationRequest request)
        {
            try
            {
                var authResponse = await _repository.Identity.RegisterAsync(request.UserName, request.Email, request.Password);

                if (!authResponse.Succeeded)
                    return BadRequest(new AuthFailedResponse
                    {
                        Errors = authResponse.Errors
                    }
                    );

                return Ok(new AuthSuccessResponse
                {
                    UserId = authResponse.UserId,
                    Email = authResponse.Email,
                    ExpiredIn = authResponse.ExpiredIn,
                    Token = authResponse.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthFailedResponse
                {
                    Errors = new string[] { $"Internal server error ({ex.Message})" }
                }
                );
            }
        }

        //Post api/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var authResponse = await _repository.Identity.LoginAsync(request.Email, request.Password);

                if (!authResponse.Succeeded)
                    return BadRequest(new AuthFailedResponse
                    {
                        Errors = authResponse.Errors
                    }
                    );

                return Ok(new AuthSuccessResponse
                {
                    UserId = authResponse.UserId,
                    Email = authResponse.Email,
                    ExpiredIn = authResponse.ExpiredIn,
                    Token = authResponse.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthFailedResponse
                {
                    Errors = new string[] { $"Internal server error ({ex.Message})" }
                });
            }
        }
    }
}