using Microsoft.AspNetCore.Mvc;
using SSECSAPI.DTO;
using SSECSAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    

    public AuthController(IAuthService authService, IEmailService emailService)
    {
        _authService = authService;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Login model)
    {
        var result = _authService.Authenticate(model);


        return Ok(new
        {
            result.Token,
            result.User,
            result.Role,
            result.StatusCode
        });
    }
    [HttpPost("signup")]
    public IActionResult Signup([FromBody] Signup model)
    {
        var message = _authService.Register(model);
        if (message == "User with this email already exists.")
            return BadRequest(new { Message = message });

        return Ok(new { Message = message });
    }

    //Tasting Email function
    //[HttpPost("send")]
    //public async Task<IActionResult> Send([FromQuery] string to, [FromQuery] string subject, [FromQuery] string body)
    //{
    //    await _emailService.SendEmailAsync(to, subject, body);
    //    return Ok("Email sent successfully!");
    //}


}
