using Microsoft.EntityFrameworkCore;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Data;
using OutOfOffice.Components.Repository.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OutOfOffice.Components.Repository
{
    public class LeaveRequestRepository : GenericRepositoryBase<LeaveRequestData>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
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
            return await base.Update(cleanLeaveRequest);
        }
    }
}
