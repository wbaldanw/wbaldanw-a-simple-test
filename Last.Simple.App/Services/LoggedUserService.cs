using Last.Simple.App.Domain.Contracts.Services;
using System.Security.Claims;

namespace Last.Simple.App.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoggedUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public long? GetUserId()
        {
            long userId;

            if (!long.TryParse(httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value, out userId))
                return null;

            return userId;
        }
    }
}
