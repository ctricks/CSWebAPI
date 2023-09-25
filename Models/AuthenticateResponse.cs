using SampleWebAPI.Models;

namespace CSWebAPI.Models
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Users user, string token)
        {
            Id = user.Id;
            PhoneNumber = user.PhoneNumber;
            Role = user.Role;
            Password = user.Password;
            Token = token;
        }
    }
}
