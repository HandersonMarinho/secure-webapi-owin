using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SecureWebApi
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// User service used to validate auth credentials.
        /// </summary>
        public IUserService UserService { get; set; }

        /// <summary>
        /// Marks this context as validated by the application
        /// </summary>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        /// <summary>
        /// Check received auth credentials in order to validate it.
        /// </summary>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            try
            {
                // Set CORS for domain external request.
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                // Search the user given the credentials.
                UserService.Find(context.UserName, context.Password);

                // Create authentication identity.
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("role", "user"));

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
            }
        }
    }
}