using Microsoft.EntityFrameworkCore;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Common;
using OutOfOffice.Components.Data;
using OutOfOffice.Components.Repository.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OutOfOffice.Components.Repository
{
    public class LeaveRequestRepository : GenericRepositoryBase<LeaveRequestData>, ILeaveRequestRepository
    {
        protected readonly IApprovalRequestRepository approvalRequestRepository;
        public LeaveRequestRepository(ApplicationDbContext dbContext, IApprovalRequestRepository approvalRequestRepository) : base(dbContext)
        {
            this.approvalRequestRepository = approvalRequestRepository;
        }

        public async Task<List<LeaveRequestData>> GetAllLeaveRequests()
        {
            IQueryable<LeaveRequestData> querable = GetQuerable()
                .Include(x => x.EmployeeData);
            return await FindAsync(querable);
        }

        public async Task<LeaveRequestData> GetAllLeaveRequestsById(long id)
        {
            IQueryable<LeaveRequestData> querable = GetQuerable()
                .Include(x => x.EmployeeData)
                .Where(x=> x.ID == id);
            return await FindSingleAsync(querable);
        }

        public async Task<List<LeaveRequestData>> GetAllLeaveRequestByEmployee(long employeeId)
        {
            IQueryable<LeaveRequestData> querable = GetQuerable()
                .Where(leaveRequest => leaveRequest.EmployeeId == employeeId)
                .Include(x => x.EmployeeData);
            return await FindAsync(querable);
        }
        public async Task<List<LeaveRequestData>> GetAllLeaveRequestHRManager(long managerId)
        {
            IQueryable<LeaveRequestData> querable = GetQuerable()
                .Include(x => x.EmployeeData)
                .Where(x => x.EmployeeData.Manager == managerId);
            return await FindAsync(querable);
        }

        public async Task<List<LeaveRequestData>> GetAllLeaveRequestProjectManager(long managerId)
        {
            IQueryable<LeaveRequestData> querable = GetQuerable()
                .Include(x => x.EmployeeData)
                .ThenInclude(y => y.ProjectInformation)
                .Where(x => x.EmployeeData.ProjectInformation.Manager == managerId);
            return await FindAsync(querable);
        }

        public async Task<bool> Create(LeaveRequestData leaveRequest)
        {
            if ((leaveRequest.End_Date - leaveRequest.Start_Date).TotalDays > leaveRequest.EmployeeData.Vacation)
            {
                return false;
            }
            LeaveRequestData cleanLeaveRequest = new LeaveRequestData();
            cleanLeaveRequest.Copy(leaveRequest); 
            return await base.Create(cleanLeaveRequest);
        }

        public async Task<bool> Update(LeaveRequestData leaveRequest)
        {
            if ((leaveRequest.End_Date - leaveRequest.Start_Date).TotalDays > leaveRequest.EmployeeData.Vacation)
            {
                return false;
            }
            LeaveRequestData cleanLeaveRequest = new LeaveRequestData();
            cleanLeaveRequest.Copy(leaveRequest);
            bool result = await base.Update(cleanLeaveRequest);
            if (result && leaveRequest.Status == LeaveRequestStatus.Submitted)
            {
                await approvalRequestRepository.CreateApprovalRequestFromLeaveRequest(leaveRequest);
            }
            return result;

        }
    }
}
