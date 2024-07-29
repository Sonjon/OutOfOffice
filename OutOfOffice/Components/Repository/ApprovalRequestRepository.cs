using Microsoft.EntityFrameworkCore;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Data;
using OutOfOffice.Components.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OutOfOffice.Components.Repository
{
    public class ApprovalRequestRepository : GenericRepositoryBase<ApprovalRequestData>, IApprovalRequestRepository
    {
        public ApprovalRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ApprovalRequestData>> GetAllApprovalRequests()
        {
            IQueryable<ApprovalRequestData> querable = GetQuerable()
                .Include(project => project.ApproverData);

            return await FindAsync(querable);
        }

        public async Task<ApprovalRequestData> GetAllApprovalRequestsById(long id)
        {
            IQueryable<ApprovalRequestData> querable = GetQuerable()
                .Include(approval => approval.LeaveRequest)
                .Where(approval => approval.ID == id);
            return await FindSingleAsync(querable);
        }

        public async Task<List<ApprovalRequestData>> GetAllApprovalRequestByEmployee(long employeeId)
        {
            IQueryable<ApprovalRequestData> querable = GetQuerable()
                .Include(approval => approval.LeaveRequest)
                .Where(approval => approval.LeaveRequest.EmployeeId == employeeId);
            return await FindAsync(querable);
        }
        public async Task<List<ApprovalRequestData>> GetAllApprovalRequestHRManager(long managerId)
        {
            IQueryable<ApprovalRequestData> querable = GetQuerable()
                .Include(approval => approval.LeaveRequest)
                .ThenInclude(x => x.EmployeeData)
                .Where(approval => approval.LeaveRequest.EmployeeData.Manager == managerId);
            return await FindAsync(querable);
        }

        public async Task<List<ApprovalRequestData>> GetAllApprovalRequestProjectManager(long managerId)
        {
            IQueryable<ApprovalRequestData> querable = GetQuerable()
                .Include(approval => approval.LeaveRequest)
                .ThenInclude(x => x.EmployeeData)
                .ThenInclude(y => y.ProjectInformation)
                .Where(x => x.LeaveRequest.EmployeeData.ProjectInformation.Manager == managerId);
            return await FindAsync(querable);
        }

        public async Task<bool> CreateApprovalRequestFromLeaveRequest(LeaveRequestData leaveRequest)
        {
            ApprovalRequestData approval = new ApprovalRequestData();
            approval.LeaveRequestId = leaveRequest.ID;
            approval.Status = leaveRequest.Status;

            return await this.Create(approval);
        }
    }
}
