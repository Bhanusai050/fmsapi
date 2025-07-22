using FmsAPI.Data;
using FmsAPI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using FmsAPI.Models;
using System.Data.Entity;


namespace FmsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly FarmManagementSystemEntities _context;

        public UserController()
        {
            _context = new FarmManagementSystemEntities();
        }

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/register
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(User user)
        {
            var result = await _userService.RegisterAsync(user);
            return Ok(result);
        }

        // POST: api/users/login
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(LoginModel model)
        {
            var result = await _userService.LoginAsync(model.Email, model.Password);

            if (!result.Success)
            {
                return Content(HttpStatusCode.Unauthorized, result);
            }

            return Ok(result);
        }

        // POST: api/users/send-otp
        [HttpPost]
        [Route("send-otp")]
        public IHttpActionResult SendOtp([FromBody] EmailRequestModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email))
                return BadRequest("Email is required.");

            var result = _userService.SendOtpAsync(model.Email);
            return Ok(result);
        }




        [HttpPost]
        [Route("verify-otp")]
        public async Task<IHttpActionResult> VerifyOtp([FromBody] OtpModel model)
        {
            bool isValid = await _userService.VerifyOtpAsync(model.Email, model.Otp);
            return Ok(new { Success = isValid });
        }

        [HttpPost]
        [Route("api/reset-password")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return NotFound();

            user.PasswordHash = HashPassword(model.NewPassword); // Replace with your actual hashing
            user.ResetOtp = null;
            user.OtpExpiry = null;

            await _context.SaveChangesAsync(); // Persist to DB

            return Ok("Password reset successful.");
        }

        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }


    }
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }


}
