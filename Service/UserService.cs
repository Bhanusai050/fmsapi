using FmsAPI.Data;
using FmsAPI.Helpers;
using FmsAPI.Interface;
using FmsAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FmsAPI.Service
{
    public class UserService : IUserService
    {
        private readonly FarmManagementSystemEntities _context;

        public UserService(FarmManagementSystemEntities context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(User user)
        {
            // Check if user exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return "User already exists";
            }

            // Hash the password before saving
            user.PasswordHash = HashHelper.ComputeHash(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "User registered successfully";
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return "Invalid email";
            }

            // Compute the hash of the input password and compare
            var hashedInputPassword = HashHelper.ComputeHash(password);
            if (user.PasswordHash != hashedInputPassword)
            {
                return "Invalid password";
            }

            return "Login successful";
        }

        public async Task<string> SendOtpAsync(string email)
        {
            // Example OTP logic (for simplicity)
            if (!_context.Users.Any(u => u.Email == email))
            {
                return "Email not found";
            }

            // TODO: Generate and send OTP
            return "OTP sent to email";
        }
    }
}


