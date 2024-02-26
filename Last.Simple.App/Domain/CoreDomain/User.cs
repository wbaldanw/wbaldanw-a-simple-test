using Last.Simple.App.Domain.DataObjects;

namespace Last.Simple.App.Domain.CoreDomain
{
    public class User
    {
        public User(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("The user name is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The password is required");

            Password = password;
            UserName = userName;
        }

        public long Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public static User FromDataObject(UserDO data) 
        {
            return new User(data.UserName, data.Password)
            {
                Id = data.Id
            };
        }
    }
}
