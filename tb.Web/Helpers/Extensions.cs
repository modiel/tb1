using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace tb.Web
{
    public static class Extensions
    {
        // -------------------------- VIEW Authorisation Helper -------------------------//
        // ClaimsPrincipal - HasOneOfRoles extension method to check if a user has any of the roles in a comma separated string
        public static bool HasOneOfRoles(this ClaimsPrincipal claims, string rolesString)
        {
            // split string into an array of roles
            var roles = rolesString.Split(",");

            // linq query to check that ClaimsPrincipal has one of these roles
            return roles.FirstOrDefault(role => claims.IsInRole(role)) != null;
        }

        // --------------------------- AUTHENTICATION Helper ----------------------------//
        // IServiceCollection extension method adding cookie authentication 
        public static void AddCookieAuthentication(this IServiceCollection services, 
                                                        string notAuthorised = "/User/ErrorNotAuthorised", 
                                                        string notAuthenticated= "/User/ErrorNotAuthenticated")
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => {
                        options.AccessDeniedPath = notAuthorised;
                        options.LoginPath = notAuthenticated;
            });
        }
        
    }
}
