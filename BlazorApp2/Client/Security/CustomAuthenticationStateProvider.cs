using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp2.Client.Security
{
    public class CustomAuthenticationStateProvider: AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userID = "1";

            var claims = new List<Claim>();

            if (userID == null)
            {
                claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "mmm"),
            };
                var anonymous1 = new ClaimsIdentity(claims, "testAuthType");
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous1)));
            }

            claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userID),
                new Claim(ClaimTypes.Role, userID)
            };
            var anonymous = new ClaimsIdentity(claims, "testAuthType");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }

        public void AuthenticateUser(string userIdentifier)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userIdentifier),
            }, "Custom Authentication");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
