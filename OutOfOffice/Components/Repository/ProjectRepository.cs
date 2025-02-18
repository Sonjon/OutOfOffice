﻿using Microsoft.EntityFrameworkCore;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Data;
using OutOfOffice.Components.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OutOfOffice.Components.Repository
{
    public class ProjectRepository : GenericRepositoryBase<ProjectData>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ProjectData> GetProject(long projectId)
        {
            IQueryable<ProjectData> querable = GetQuerable().Where(project => project.ID == projectId)
                        .Include(project => project.ManagerData)
                        .Include(project => project.EmployeesList);

            return await FindSingleAsync(querable);
        }

        public async Task<List<ProjectData>> GetProjectAsList(long projectId)
        {
            IQueryable<ProjectData> querable = GetQuerable().Where(project => project.ID == projectId)
                        .Include(project => project.ManagerData)
                        .Include(project => project.EmployeesList);

            return await FindAsync(querable);
        }

        public async Task<List<ProjectData>> GetAllProject()
        {
            IQueryable<ProjectData> querable =GetQuerable()
                    .Include(project => project.ManagerData)
                    .Include(project => project.EmployeesList);

            return await FindAsync(querable);
        }

        public async Task<List<ProjectData>> GetAllHRManagerEmployeeProject(long managerID)
        {
            IQueryable<ProjectData> querable = GetQuerable()
                        .Include(project => project.ManagerData)
                        .Include(project => project.EmployeesList)
                        .Where(project => project.EmployeesList != null && project.EmployeesList.Any(employee => employee.Manager == managerID));

            return await FindAsync(querable);
        }

        public async Task<List<ProjectData>> GetAllProjectForProjectManager(long managerID)
        {
            IQueryable<ProjectData> querable = GetQuerable().Where(project => project.Manager == managerID)
                        .Include(project => project.ManagerData)
                        .Include(project => project.EmployeesList);

            return await FindAsync(querable);
        }

        public async Task<bool> Create(ProjectData project)
        {
            ProjectData cleanProject = new ProjectData();
            cleanProject.Copy(project);
            return await base.Create(cleanProject);
        }

        public async Task<bool> Update(ProjectData project)
        {
            ProjectData cleanProject = new ProjectData();
            project.EmployeesList = null;
            cleanProject.Copy(project);
            return await base.Update(cleanProject);
        }
    }
}
