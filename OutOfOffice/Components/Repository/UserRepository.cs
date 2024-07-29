using Microsoft.EntityFrameworkCore;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Common;
using OutOfOffice.Components.Data;
using OutOfOffice.Components.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OutOfOffice.Components.Repository
{
    public class UserRepository : GenericRepositoryBase<UserData>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<EmployeeData> Login(LoginRequest loginRequest)
        {
           IQueryable<UserData> querable = GetQuerable().Where(user => user.Username == loginRequest.UserName && user.Password == loginRequest.Password)
                .Include(user => user.Employee);

           UserData userData = await FindSingleAsync(querable);

            return userData.Employee;
        }

    }
}
