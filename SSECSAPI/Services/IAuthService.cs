using SSECSAPI.DTO;

namespace SSECSAPI.Services
{
    public interface IAuthService
    {
        AuthResponse Authenticate(Login model);
        string Register(Signup model);
       

    }
}
