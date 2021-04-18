using System.Threading.Tasks;
using Entities.Results;

namespace Contracts.Repositories
{
    public interface IIdentityRepository
    {
        Task<AuthenticationResult> RegisterAsync(string userName, string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}