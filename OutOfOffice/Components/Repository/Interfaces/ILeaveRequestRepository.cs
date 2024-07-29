using OutOfOffice.Components.Data;
﻿using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface ILeaveRequestRepository : IGenericRepositoryBase<LeaveRequestData>
    {
        Task<List<LeaveRequestData>> GetAllLeaveRequests();
        Task<List<LeaveRequestData>> GetAllLeaveRequestByEmployee(long employeeId);
        Task<List<LeaveRequestData>> GetAllLeaveRequestHRManager(long managerID);
        Task<List<LeaveRequestData>> GetAllLeaveRequestProjectManager(long managerID);
        Task<LeaveRequestData> GetAllLeaveRequestsById(long id);

        /*Task<EmployeeData> GetEmployee(long employeeId);
Task<List<EmployeeData>> GetAllHRManager();
Task<List<EmployeeData>> GetAllEmployeeHRManager(long managerID);
Task<List<EmployeeData>> GetAllEmployeeProjectManager(long managerID);*/
        Task<bool> Create(LeaveRequestData leaveRequestData);

        Task<bool> Update(LeaveRequestData leaveRequestData);
    }
}
