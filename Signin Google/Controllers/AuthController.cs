using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Signin_Google.Dtos;
using Signin_Google.Services;

namespace Signin_Google.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public AuthenticationController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("google-signin")]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _googleAuthService.GoogleSignInAsync(model);
        if (response.Errors.Any())
            return BadRequest(response.Errors);

        return Ok(response.Data);
    }


    [HttpGet("google-signin")]
    public async Task<IActionResult> GoogleSignIn([FromQuery] string idToken)
    {
        if (string.IsNullOrEmpty(idToken))
            return BadRequest("ID Token is required.");

        var model = new GoogleSignInVM { IdToken = idToken };
        var response = await _googleAuthService.GoogleSignInAsync(model);
        

        if (response.Errors.Any())
            return BadRequest(response.Errors);

        return Ok(response.Data);
    }

}
