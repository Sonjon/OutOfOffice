using OutOfOffice.Components.Data;

namespace OutOfOffice.Components.Common
{
    public interface IAuthStorage
    {
        Task<UserAuthData?> GetAuthDataAsync();
        Task StoreAuthData(UserAuthData authData);
        Task ClearAuthData();
        Task<bool> IsLogedInAsync(string scope);
        Task<bool> IsUserLogedInAsync();
    }
}
