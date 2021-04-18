using Contracts.Repositories;
using Entities.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Repository
{
    public class IdentityRepositoryWrapper : IIdentityRepositoryWrapper
    {
        private UserManager<IdentityUser> _userManager;
        private IOptions<JwtSettings> _jwtSettings;
        private IdentityRepository _identity;

        public IIdentityRepository Identity
        {
            get
            {
                if (_identity is null)
                    _identity = new IdentityRepository(_userManager, _jwtSettings);

                return _identity;
            }
        }

        public IdentityRepositoryWrapper(UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager= userManager;
            _jwtSettings = jwtSettings;
        }
    }
}