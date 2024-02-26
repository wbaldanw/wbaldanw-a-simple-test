namespace Last.Simple.App.Domain.Models.Users
{
    public class SignInRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
