using Microsoft.EntityFrameworkCore;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Common;
using OutOfOffice.Components.Data;
using OutOfOffice.Components.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OutOfOffice.Components.Repository
{
    public class EmployeeRepository : GenericRepositoryBase<EmployeeData>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EmployeeData> GetEmployee(long employeeId)
        {
            IQueryable<EmployeeData> querable = GetQuerable().Where(employee => employee.ID == employeeId)
                .Include(employee => employee.ManagerData)
                .Include(employee => employee.ProjectInformation);
            return await FindSingleReadOnlyAsync(querable);
        }

        public async Task<List<EmployeeData>> GetAllEmployees()
        {
            IQueryable<EmployeeData> querable = GetQuerable()
                .Include(manager => manager.ManagerData);
            return await FindAsync(querable);
        }

        public async Task<List<EmployeeData>> GetAllHRManager()
        {
            IQueryable<EmployeeData> querable = GetQuerable()
                .Where(employee => employee.Position == Positions.HR_MANAGER);
                
            return await FindAsync(querable);
        }

        public async Task<List<EmployeeData>> GetAllProjectManager()
        {
            IQueryable<EmployeeData> querable = GetQuerable()
                .Where(employee => employee.Position == Positions.PROJECT_MANAGER);
                
            return await FindAsync(querable);
        }

        public async Task<List<EmployeeData>> GetAllEmployeeHRManager(long managerID)
        {
            IQueryable<EmployeeData> querable = GetQuerable()
                .Where(employee => employee.Manager == managerID)
                .Include(manager => manager.ManagerData);
            return await FindAsync(querable);
        }

        public async Task<List<EmployeeData>> GetAllEmployeeProjectManager(long managerID)
        {
            IQueryable<EmployeeData> querable = GetQuerable()
                .Include(employee => employee.ProjectInformation)
                .Where(employee => employee.ProjectInformation.Manager == managerID);
            return await FindAsync(querable);
        }

        public async Task<bool> Create(EmployeeData employee)
        {
            EmployeeData cleanEmployee = new EmployeeData();
            cleanEmployee.Copy(employee);
            return await base.Create(cleanEmployee);
        }

        public async Task<bool> Update(EmployeeData employee)
        {
            EmployeeData cleanEmployee = new EmployeeData();
            cleanEmployee.Copy(employee);
            return await base.Update(cleanEmployee);
        }

    }
}
