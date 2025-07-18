namespace FmsAPI.Models
{
    public class LoginResponseModel
    {
        public bool Success { get; set; }         // ✅ Fixes "no definition for 'Success'"
        public string Message { get; set; }
        public string Token { get; set; }         // Optional: for JWT or future use
        public string Username { get; set; }      // Optional: display username in UI
        public string Email { get; set; }         // Optional
    }
}
