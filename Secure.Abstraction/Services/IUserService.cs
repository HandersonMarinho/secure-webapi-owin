using System.Security;

namespace Secure.Abstraction
{
    public interface IUserService
    {
        User Find(string userName, SecureString password);
    }
}
