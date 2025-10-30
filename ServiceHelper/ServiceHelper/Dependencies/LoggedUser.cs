using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    public class LoggedUser
    {
        private readonly HttpContext _context;

        public bool IsAuthenticated => (_context.User?.Identity?.IsAuthenticated).GetValueOrDefault();

        public long Id
        {
            get
            {
                if (!long.TryParse(_context.User.FindFirst("sub")?.Value, out var result))
                {
                    throw new LoggedUserException("User is not authenticated. User should be authenticated to fetch user id.");
                }

                return result;
            }
        }

        public string Name => _context.User?.FindFirst("name")?.Value ?? throw new LoggedUserException("User is not authenticated. User should be authenticated to fetch user name.");

        public Guid ClientAppId
        {
            get
            {
                if (!Guid.TryParse(_context.User?.FindFirst("client_id")?.Value, out var result))
                {
                    throw new LoggedUserException("User is not authenticated. User should be authenticated to fetch client app id.");
                }

                return result;
            }
        }

        public Zouid? Zouid
        {
            get
            {
                string value = _context.User?.FindFirst("zouid")?.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    return Zouid.Parse(value);
                }

                return null;
            }
        }

        //public List<Permission> Permissions => Permission.ConvertFromString(_context.User?.FindFirst("permissions")?.Value ?? string.Empty);

        public LoggedUser(IHttpContextAccessor context)
        {
            _context = context.HttpContext;
        }
    }
}
