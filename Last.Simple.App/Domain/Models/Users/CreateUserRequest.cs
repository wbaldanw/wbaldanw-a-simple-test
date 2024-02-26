namespace Last.Simple.App.Domain.Models.Users
{
    public class CreateUserRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
