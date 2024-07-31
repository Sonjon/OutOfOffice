using OutOfOffice.Components.Data;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface IApprovalRequestRepository : IGenericRepositoryBase<ApprovalRequestData>
    {
        Task<List<ApprovalRequestData>> GetAllApprovalRequests();
        Task<List<ApprovalRequestData>> GetAllApprovalRequestByEmployee(long employeeId);
        Task<List<ApprovalRequestData>> GetAllApprovalRequestHRManager(long managerID);
        Task<List<ApprovalRequestData>> GetAllApprovalRequestProjectManager(long managerID);
        Task<ApprovalRequestData> GetAllApprovalRequestsById(long id);
        Task<bool> CreateApprovalRequestFromLeaveRequest(LeaveRequestData leaveRequest);
        Task<bool> Update(ApprovalRequestData leaveRequestData);
    }
}
