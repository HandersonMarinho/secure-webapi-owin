using System;

/// <summary>
/// Fake service used to test injection inside of http pipeline.
/// </summary>
namespace Secure.Api
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public interface IUserService
    {
        User Find(string userName, string password);
    }

    public class UserService : IUserService
    {
        public User Find(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException($"{nameof(userName)} is mandatory.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"{nameof(password)} is mandatory.");

            if (userName == "admin" && password == "admin")
            {
                return new User { Id = Guid.NewGuid(), UserName = userName };
            }
            else
            {
                throw new InvalidOperationException("The user name or password is incorrect.");
            }
        }
    }
}