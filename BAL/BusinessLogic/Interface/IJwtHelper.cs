using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BAL.BusinessLogic.Interface
{
    public interface IJwtHelper
    {
        string GenerateToken(string username, string role);
    }
}
