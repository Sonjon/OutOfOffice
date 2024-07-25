using OutOfOffice.Components.Data;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface IEmployeeRepository : IGenericRepositoryBase<EmployeeData>
    {
        Task<EmployeeData> GetEmployee(long employeeId);
        Task<List<EmployeeData>> GetAllEmployees();
        Task<List<EmployeeData>> GetAllHRManager();
        Task<List<EmployeeData>> GetAllProjectManager();
        Task<List<EmployeeData>> GetAllEmployeeHRManager(long managerID);
        Task<List<EmployeeData>> GetAllEmployeeProjectManager(long managerID);
    }
}
