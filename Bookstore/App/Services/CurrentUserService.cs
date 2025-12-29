using System.Security.Claims;
using Bookstore.App.Services.Interfaces;

namespace Bookstore.App.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    throw new UnauthorizedAccessException();

                return int.Parse(userIdClaim);
            }
        }
    }
}
