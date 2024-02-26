namespace Last.Simple.App.Domain.CoreDomain
{
    public class SignIn
    {
        private readonly User? user;

        public SignIn(User? user)
        {
            this.user = user;
        }

        public bool IsAuthenticated(string userName, string password)
        {
            return user?.UserName == userName && user?.Password == password;
        }
    }
}
