using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;
using System.Security.Claims;

namespace OutOfOffice.Components.Data
{
    public class UserAuthData
    {
        public readonly struct AuthKeys
        {
            public const string USER_NAME = "USER_NAME";
            public const string ROLE = "ROLE";
            public const string IS_LOGGED = "IS_LOGGED";
            public const string EMPLOYEE_ID = "EMPLOYEE_ID";
        }

        public struct Roles
        {
            public const string EMPLOYEE = "EMPLOYEE";
            public const string HR_MANAGER = "HR_MANAGER";
            public const string PROJECT_MANAGER = "PROJECT_MANAGER";
            public const string ADMINISTRATOR = "ADMINISTRATOR";
        }

        public string UserName { get; set; }
        public long EmployeeId { get; set; }
        public string Role { get; set; }
        public List<Claim> Claims { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Claim[] claims = Claims.ToArray();
            ClaimsIdentity identity = new ClaimsIdentity(claims, "WebAppAuth");

            claimsPrincipal.AddIdentity(identity);

            return claimsPrincipal;
        }

    }

}
