using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.AuthenticationServer.Services
{
    public interface IDbInitializerService
    {
         Task InitializeAsync();
    }
}
