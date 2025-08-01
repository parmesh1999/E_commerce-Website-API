using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using SSECSAPI.Data;
using SSECSAPI.DTO;
using SSECSAPI.Helpers;
using SSECSAPI.Models;
using SSECSAPI.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
   

    public AuthService(IOptions<JwtSettings> options, AppDbContext context, IEmailService emailService)
    {
        _jwtSettings = options.Value;
        _context = context;
        _emailService = emailService;
        
    }

    public AuthResponse Authenticate(Login model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            return new AuthResponse
            {
                StatusCode = "401"
            };
        }

        var userRole = _context.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
        if (userRole != null)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == userRole.RoleId);
            var token = JwtTokenGenerator.GenerateToken(user.Email, _jwtSettings);
            return new AuthResponse
            {
                Token = token,
                User = user,
                Role = role,
                StatusCode = "200"
            };
        }
        else
        {
            return new AuthResponse
            {
                StatusCode = "403"
            };
        }
    }

    public string Register(Signup model)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (existingUser != null)
        {
            return "User with this email already exists.";
        }

        var user = new User
        {
            Name = model.Name,
            Mobile = model.Mobile,
            Email = model.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // ✅ Send welcome email
        string subject = "Welcome to SSECS!";
        string body = $@"
        <h2>Hello {user.Name},</h2>
        <p>Thank you for registering with SSECS.</p>
        <p>We're glad to have you on board.</p>
        <br/>
        <strong>Login Email:</strong> {user.Email}
        <br/>
        <strong>Password:</strong> {user.Password}
        <br/>
        <p>Regards,<br/>SSECS Team</p>";

        _emailService.SendEmailAsync(user.Email, subject, body);

        return "User registered and welcome email sent.";

    }
}