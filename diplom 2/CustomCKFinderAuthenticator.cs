using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CKSource.CKFinder.Connector.Core;
using CKSource.CKFinder.Connector.Core.Authentication;

namespace diplom_2
{
    public class CustomCKFinderAuthenticator : IAuthenticator
    {
        public Task<IUser> AuthenticateAsync(ICommandRequest commandRequest, CancellationToken cancellationToken)
        {
            var claimsPrincipal = commandRequest.Principal as ClaimsPrincipal;
            var roles = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
            /*
             * Enable CKFinder only for authenticated users.
             */
            var isAuthenticated = claimsPrincipal.Identity.IsAuthenticated;
            var user = new User(isAuthenticated, roles);
            return Task.FromResult((IUser)user);
        }
    }
}