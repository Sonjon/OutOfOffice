using OutOfOffice.Components.Data;
﻿using OutOfOffice.Components.Common;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepositoryBase<UserData>
    {
        Task<EmployeeData> Login(LoginRequest loginRequest);

    }
}
