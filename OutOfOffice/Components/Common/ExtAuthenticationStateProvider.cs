using Microsoft.AspNetCore.Components.Authorization;
using OutOfOffice.Components.Data;
using System.Security.Claims;
using OutOfOffice.Components.Backend;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using static OutOfOffice.Components.Data.UserAuthData;

namespace OutOfOffice.Components.Common
{
    public class ExtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthStorage authStorage;

        public ExtAuthenticationStateProvider(IAuthStorage authStorage)
        {
            this.authStorage = authStorage;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            UserAuthData? authData = await authStorage.GetAuthDataAsync();
            var principal = new ClaimsPrincipal();
            if (authData != null)
            {
                principal = authData.ToClaimsPrincipal();
            }
            return new AuthenticationState(principal);
        }

        public async Task LogIn(LoginRequest loginRequest)
        {
            Employee userEmployee = await Backend.Backend.Login(loginRequest);
            if (userEmployee != null)
            {
                UserAuthData authData = new UserAuthData();
                authData.UserName = loginRequest.UserName;
                authData.Role = userEmployee.Position.Replace(" ","_").ToUpper();
                authData.EmployeeId = userEmployee.ID;

                await authStorage.StoreAuthData(authData);
                await ReloadAuthenticationStateAsync();
            }
        }

        public async Task LogOut()
        {
            await authStorage.ClearAuthData();
            await ReloadAuthenticationStateAsync();
        }

        private async Task ReloadAuthenticationStateAsync()
        {
                AuthenticationState authenticationState = await GetAuthenticationStateAsync();
                NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }
    
        public async Task<bool> IsInRole(string role)
        {
            bool result = false;
            UserAuthData? authData = await authStorage.GetAuthDataAsync();
            if (authData != null)
            {
                if(authData.Role == role)
                    result = true;
            }
            return result;
        }

        public async Task<string> GetRole()
        {
            UserAuthData? authData = await authStorage.GetAuthDataAsync();
            return authData.Role;
        }

        public async Task<long?> GetYourID()
        {
            UserAuthData? authData = await authStorage.GetAuthDataAsync();
            if (authData != null)
                return authData.EmployeeId;
            return null;
        }
    }
}
