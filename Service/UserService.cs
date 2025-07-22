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
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return "Email already registered.";
            }

            user.PasswordHash = HashHelper.HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ? Send welcome email after registration
            try
            {
                MailService.SendWelcomeEmail(user.Email, user.Username);
            }
            catch (Exception ex)
            {
                return $"Registered, but email failed: {ex.Message}";
            }

            return "Registration successful. Welcome email sent.";
        }

        public async Task<LoginResponseModel> LoginAsync(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !HashHelper.VerifyPassword(password, user.PasswordHash))
            {
                return new LoginResponseModel
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            return new LoginResponseModel
            {
                Success = true,
                Message = "Login successful.",
                Username = user.Username,
                Email = user.Email
                // Token = "generate JWT here if needed"
            };
        }


        public async Task<string> SendOtpAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return "User not found";

            string otp = new Random().Next(100000, 999999).ToString(); // 6-digit OTP
            DateTime expiry = DateTime.Now.AddMinutes(5); // 5 minutes validity

            user.ResetOtp = otp;
            user.OtpExpiry = expiry;

            _context.SaveChanges();

            string subject = "Your OTP for Password Reset";
            string body = $"<p>Dear {user.Username},</p><p>Your OTP is <b>{otp}</b>. It will expire in 5 minutes.</p>";

            try
            {
                MailService.SendEmail(user.Email, subject, body);
                return "OTP sent successfully";
            }
            catch (Exception ex)
            {
                return $"Failed to send OTP: {ex.Message}";
            }
        }


        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var token = _context.PasswordResetTokens
                .Where(t => t.Email == email && t.OTP == otp && t.IsUsed == false && t.ExpiryTime > DateTime.Now)
                .OrderByDescending(t => t.ExpiryTime)
                .FirstOrDefault();

            if (token == null)
            {
                return false;
            }

            token.IsUsed = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> ResetPasswordAsync(string email, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return "Email not found.";

            user.PasswordHash = HashHelper.HashPassword(newPassword);
            user.ResetOtp = null;
            user.OtpExpiry = null;
            await _context.SaveChangesAsync();

            return "Password reset successful.";
        }


        // ? Method required by interface
        public async Task<string> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                MailService.SendEmail(toEmail, subject, body);
                return "Email sent successfully.";
            }
            catch (Exception ex)
            {
                return $"Email failed: {ex.Message}";
            }
        }
    }
}
