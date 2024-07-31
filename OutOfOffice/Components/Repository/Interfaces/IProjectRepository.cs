using OutOfOffice.Components.Data;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface IProjectRepository : IGenericRepositoryBase<ProjectData>
    {
        Task<ProjectData> GetProject(long projectId);
        Task<List<ProjectData>> GetProjectAsList(long projectId);
        Task<List<ProjectData>> GetAllProject();
        Task<List<ProjectData>> GetAllHRManagerEmployeeProject(long managerID);
        Task<List<ProjectData>> GetAllProjectForProjectManager(long managerID);
        Task<bool> Create(ProjectData leaveRequestData);
        Task<bool> Update(ProjectData leaveRequestData);

    }
}
