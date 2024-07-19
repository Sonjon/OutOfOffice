using OutOfOffice.Components.Data;
using static System.Formats.Asn1.AsnWriter;
using System.Security.Claims;

namespace OutOfOffice.Components.Common
{
    public class AuthStorage : IAuthStorage
    {

        private readonly ILocalStorageAccessor localStorageAccessor;

        public AuthStorage(ILocalStorageAccessor localStorageAccessor)
        {
            this.localStorageAccessor = localStorageAccessor;
        }



        public async Task StoreAuthData(UserAuthData authData)
        {
            await localStorageAccessor.SetValueAsync(UserAuthData.AuthKeys.USER_NAME, authData.UserName);
            await localStorageAccessor.SetValueAsync(UserAuthData.AuthKeys.ROLE, authData.Role);
            await localStorageAccessor.SetValueAsync(UserAuthData.AuthKeys.IS_LOGGED, true);
            await localStorageAccessor.SetValueAsync(UserAuthData.AuthKeys.EMPLOYEE_ID, authData.EmployeeId);
        }

        public async Task ClearAuthData()
        {
            await localStorageAccessor.RemoveAsync(UserAuthData.AuthKeys.USER_NAME);
            await localStorageAccessor.RemoveAsync(UserAuthData.AuthKeys.ROLE);
            await localStorageAccessor.SetValueAsync(UserAuthData.AuthKeys.IS_LOGGED, false);
            await localStorageAccessor.RemoveAsync(UserAuthData.AuthKeys.EMPLOYEE_ID);
        }



        public async Task<bool> IsUserLogedInAsync()
        {
            string value = await localStorageAccessor.GetValueAsync<string>(UserAuthData.AuthKeys.IS_LOGGED);
            if (Boolean.TryParse(value, out bool result))
            {
                return result;
            }
            return false;
        }


        public async Task<bool> IsLogedInAsync(string scope)
        {
            string value = await localStorageAccessor.GetValueAsync<string>(UserAuthData.AuthKeys.ROLE);
            return UserAuthData.AuthKeys.ROLE.Equals(scope);
        }


        public async Task<UserAuthData?> GetAuthDataAsync()
        {
            string role = await localStorageAccessor.GetValueAsync<string>(UserAuthData.AuthKeys.ROLE);
            string userName = await localStorageAccessor.GetValueAsync<string>(UserAuthData.AuthKeys.USER_NAME);

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(role))
            {
                return null;
            }
            string employeeId = await localStorageAccessor.GetValueAsync<string>(UserAuthData.AuthKeys.EMPLOYEE_ID);

            UserAuthData authData = new UserAuthData();
            authData.UserName = userName;
            authData.EmployeeId = int.Parse(employeeId);
            authData.Role = role;

            authData.Claims = PrepareClaims(authData);
            return authData;
        }

        private List<Claim> PrepareClaims(UserAuthData authData)
        {
            List<Claim> roleClaims = GetRoleClaims(authData);

            return new List<Claim>()
            {
                new (ClaimTypes.Name, authData.UserName)
            }.Concat(roleClaims).ToList();
        }


        private List<Claim> GetRoleClaims(UserAuthData authData)
        {
            List<Claim> roleClaims = new List<Claim>();

            switch (authData.Role)
            {
                case UserAuthData.Roles.ADMINISTRATOR:
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.ADMINISTRATOR));
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.EMPLOYEE));
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.PROJECT_MANAGER));
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.HR_MANAGER));
                    break;
                case UserAuthData.Roles.HR_MANAGER:
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.EMPLOYEE));
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.HR_MANAGER));
                    break;
                case UserAuthData.Roles.PROJECT_MANAGER:
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.EMPLOYEE));
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.PROJECT_MANAGER));
                    break;
                case UserAuthData.Roles.EMPLOYEE:
                    roleClaims.Add(new Claim(ClaimTypes.Role, UserAuthData.Roles.EMPLOYEE));
                    break;
            }

            return roleClaims;
        }
    }
}
