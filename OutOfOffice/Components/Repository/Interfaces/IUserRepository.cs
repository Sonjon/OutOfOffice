using OutOfOffice.Components.Data;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepositoryBase<UserData>
    {
        Task<EmployeeData> Login(LoginRequest loginRequest);

    }
}
