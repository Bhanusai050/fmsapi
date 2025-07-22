using FmsAPI.Data;
using FmsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FmsAPI.Interface
{
    public interface IUserService
    {
        Task<string> RegisterAsync(User user);
        Task<LoginResponseModel> LoginAsync(string email, string password);

        Task<string> SendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<string> SendEmailAsync(string toEmail, string subject, string body);
        Task<string> ResetPasswordAsync(string email, string newPassword);




    }
}