using Last.Simple.App.Domain.Models.Users;

namespace Last.Simple.App.Domain.Contracts.UseCases.Users
{
    public interface ISignInUC
    {
        Task<string> SignIn(SignInRequest request);
    }
}
