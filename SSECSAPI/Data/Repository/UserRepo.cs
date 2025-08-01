using SSECSAPI.Data.Interface;
using SSECSAPI.Models;
using SSECSAPI.Services;

namespace SSECSAPI.Data.Repository
{
    public class UserRepo : IUser
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public UserRepo(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public string AddUser(User user)
        {
            // Hash the password before saving
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();
            return "User Added Successfully!";
        }

        public string DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return "User Deleted successfully!";
            }
            return "User not found!";
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public string UpdateUser(User user)
        {
            // Hash the password again only if it is changed (you can adjust this logic)
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null)
            {
                return "User not found!";
            }

            existingUser.Name = user.Name;
            existingUser.Mobile = user.Mobile;
            existingUser.Email = user.Email;

            // Re-hash only if password changed
            if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _context.Users.Update(existingUser);
            _context.SaveChanges();

            //Emailing Function
            // ✅ Send welcome email
            string subject = "Update Your Profile!";
            string body = $@"
            <h2>Hello {user.Name},</h2>
            <p>Thank you for updating your profile.</p>
            <p>We're glad to have you on board.</p>
            <br/>
            <strong>Password:</strong> {user.Password}
            <br/>
            <p>Regards,<br/>SSECS Team</p>";

            _emailService.SendEmailAsync(user.Email, subject, body);

            return "User Updated Successfully!";
        }
    }
}
