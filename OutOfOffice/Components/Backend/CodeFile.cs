using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using OutOfOffice.Components.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace OutOfOffice.Components.Backend
{
    public class Backend
    {
        private static bool sendInputSQLCommand(string command)
        {
            SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=OutOfOffice;Integrated Security=True;");
            try
            {
                using (SqlCommand cmd = new SqlCommand(command, myConn))
                {
                    myConn.Open();
                    int rowsAdded = cmd.ExecuteNonQuery();
                    if (rowsAdded > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return false;
        }

        private static List<Dictionary<string, object>> sendSelectSQLCommand(string command)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=OutOfOffice;Integrated Security=True;");
            try
            {
                using (SqlCommand cmd = new SqlCommand(command, myConn))
                {
                    myConn.Open();
                    SqlDataReader myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        Dictionary<string, object> temp = new Dictionary<string, object>();
                        for (int i = 0; i < myReader.FieldCount; ++i)
                        {
                            temp.Add(myReader.GetName(i), myReader.GetValue(i));
                        }
                        rows.Add(temp);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return rows;
        }

        public static async Task<bool> Add_Employee(Employee newEmployee)
        {
            String str = "insert into Employee ([Full Name], [Subdivision], [Position], [Status],";
            if (newEmployee.Manager != 0)
                str += "[People Partner],";
            if (newEmployee.Project != 0)
                str += "[Project],";
            str += "[Out-of-Office Balance]) values('"+ newEmployee.Full_Name+"','"+ newEmployee.Subdivision+"','"+ newEmployee.Position+"','"+ newEmployee.Status+"','";
            if (newEmployee.Manager != 0)
                str += newEmployee.Manager+"','";
            if (newEmployee.Project != 0)
                str += newEmployee.Project + "','";
            str += newEmployee.Vacation+"')";
            return sendInputSQLCommand(str);
        }

        public static async Task<bool> Update_Employee(int id, Employee newEmployeeData, Employee oldEmployeeData)
        {
            bool coma = false;
            String str = "update Employee set ";
            if (newEmployeeData.Full_Name != oldEmployeeData.Full_Name)
            {
                str += "[Full Name] = '" + newEmployeeData.Full_Name+"'";
                coma = true;
            }

            if (newEmployeeData.Subdivision != oldEmployeeData.Subdivision)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Subdivision] = '" + newEmployeeData.Subdivision + "'";
                coma = true;
            }
            if (newEmployeeData.Position != oldEmployeeData.Position)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Position] = '" + newEmployeeData.Position + "'";
                coma = true;
            }
            if (newEmployeeData.Status != oldEmployeeData.Status)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Status] = '" + newEmployeeData.Status + "'";
                coma = true;
            }
            if (newEmployeeData.Manager != oldEmployeeData.Manager)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[People Partner] = " + newEmployeeData.Manager;
                coma = true;
            }
            if (newEmployeeData.Vacation != oldEmployeeData.Vacation)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Out-of-Office Balance] = " + newEmployeeData.Vacation;
            }
            if (newEmployeeData.Project != oldEmployeeData.Project)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Project] = " + newEmployeeData.Project;
                coma = true;
            }
            str += " WHERE ID = " + id;
            return sendInputSQLCommand(str);
        }

        public static async Task<List<Employee>> GetEmployeesAsync()
        {
            List<Employee> employees = new List<Employee>();
            String str = CreateGetEmployeesCommand();
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            Dictionary<string, string> displayName = await Helper.getFieldsDisplayNames(typeof(Employee));
            foreach(Dictionary<string, object> row in rows)
            {
                try
                {
                    Employee employee = new Employee();
                    employee.ID = (int)(long)row["ID"];
                    employee.Full_Name = row["Full Name"].ToString();
                    employee.Subdivision = row["Subdivision"].ToString();
                    employee.Position = row["Position"].ToString();
                    employee.Status = Helper.ParseEnum<ActiveStatus>(row["Status"].ToString());
                    employee.Manager = (row["People Partner"] == DBNull.Value) ? null : (int)(long)row["People Partner"];
                    employee.Manager_String = row["Manager"].ToString();
                    employee.Project = (row["Project"] == DBNull.Value) ? null : (int)(long)row["Project"];
                    employee.Vacation = (int)row["Out-of-Office Balance"];
                    employees.Add(employee);
                }
                catch {
                // write to log about error data
                }
            }

            return employees;
        }

        public static async Task<List<Employee>> GetEmployeesWithPositionAsync(string position)
        {
            List<Employee> employees = new List<Employee>();
            String str = CreateGetEmployeesCommand() + " where emp.[Position] = '" + position + "'";
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            Dictionary<string, string> displayName = await Helper.getFieldsDisplayNames(typeof(Employee));
            foreach (Dictionary<string, object> row in rows)
            {
                try
                {
                    Employee employee = new Employee();
                    employee.ID = (int)(long)row["ID"];
                    employee.Full_Name = row["Full Name"].ToString();
                    employee.Subdivision = row["Subdivision"].ToString();
                    employee.Position = row["Position"].ToString();
                    employee.Status = Helper.ParseEnum<ActiveStatus>(row["Status"].ToString());
                    employee.Manager = (row["People Partner"] == DBNull.Value) ? null : (int)(long)row["People Partner"];
                    employee.Manager_String = row["Manager"].ToString();
                    employee.Vacation = (int)row["Out-of-Office Balance"];
                    employee.Project = (row["Project"] == DBNull.Value) ? null : (int)(long)row["Project"];
                    employees.Add(employee);
                }
                catch
                {
                    // write to log about error data
                }
            }

            return employees;
        }

        public static async Task<Employee> GetEmployeeAsync(int id)
        {
            String str = CreateGetEmployeesCommand() + " where emp.ID=" + id;

            Employee employee = new Employee();

            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            Dictionary<string, object> row = rows[0];
            if (row.Count == 0)
                return null;
            try
            {
                employee.ID = (int)(long)row["ID"];
                employee.Full_Name = row["Full Name"].ToString();
                employee.Subdivision = row["Subdivision"].ToString();
                employee.Position = row["Position"].ToString();
                employee.Status = Helper.ParseEnum<ActiveStatus>(row["Status"].ToString());
                employee.Manager = (row["People Partner"] == DBNull.Value) ? null : (int)(long)row["People Partner"];
                employee.Manager_String = row["Manager"].ToString();
                employee.Vacation = (int)row["Out-of-Office Balance"];
                employee.Project = (row["Project"] == DBNull.Value) ? null : (int)(long)row["Project"];
            }
            catch
            {
                // write to log about error data
            }
            return employee;
        }

        public static async Task<List<Project>> GetProjectsAsync()
        {
            List<Project> projects = new List<Project>();
            String str = CreateGetProjectsCommand();
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            foreach (Dictionary<string, object> row in rows)
            {
                try
                {
                    Project project = new Project();
                    project.ID = (int)(long)row["ID"];
                    project.Project_Type = row["Project Type"].ToString();
                    project.Start_Date = (DateTime)row["Start Date"];
                    project.End_Date = (DateTime)row["End Date"];
                    project.Manager = (row["Project Manager"] == DBNull.Value) ? null : (int)(long)row["Project Manager"];
                    project.Manager_String = row["Manager"].ToString();
                    project.Status = Helper.ParseEnum<ActiveStatus>(row["Status"].ToString());
                    projects.Add(project);
                }
                catch
                {
                    // write to log about error data
                }
            }

            return projects;
        }

        public static async Task<Project> GetProjectAsync(int id)
        {
            String str = CreateGetProjectsCommand() + " where pr.ID=" + id;

            Project project = new Project();

            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);            
            if (rows.Count == 0)
                return null;
            try
            {
                Dictionary<string, object> row = rows[0];
                project.ID = (int)(long)row["ID"];
                project.Project_Type = row["Project Type"].ToString();
                project.Start_Date = (DateTime)row["Start Date"];
                project.End_Date = (DateTime)row["End Date"];
                project.Manager = (row["Project Manager"] == DBNull.Value) ? null : (int)(long)row["Project Manager"];
                project.Manager_String = row["Manager"].ToString();
                project.Status = Helper.ParseEnum<ActiveStatus>(row["Status"].ToString());
            }
            catch
            {
                // write to log about error data
            }
            return project;
        }


        public static async Task<bool> Add_Project(Project newproject)
        {
            String str = "insert into Project ([Project Type], [Start Date], [End Date], [Project Manager], [Comment], [Status])";
            str += " values('" + newproject.Project_Type + "','" + newproject.Start_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "','" + newproject.End_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "','";
            str += newproject.Manager + "','" + newproject.Comment + "','" + newproject.Status + "')";
            return sendInputSQLCommand(str);
        }


        public static async Task<bool> Update_Project(int id, Project newProjectData, Project oldProjectData)
        {
            bool coma = false;
            String str = "update Project set ";
            if (newProjectData.Project_Type != oldProjectData.Project_Type)
            {
                str += "[Project Type] = '" + newProjectData.Project_Type + "'";
                coma = true;
            }

            if (newProjectData.Start_Date != oldProjectData.Start_Date)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Start Date] = '" + newProjectData.Start_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "'";
                coma = true;
            }
            if (newProjectData.End_Date != oldProjectData.End_Date)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[End Date] = '" + newProjectData.End_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "'";
                coma = true;
            }
            if (newProjectData.Status != oldProjectData.Status)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Status] = '" + newProjectData.Status + "'";
                coma = true;
            }
            if (newProjectData.Manager != oldProjectData.Manager)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Project Manager] = " + newProjectData.Manager;
                coma = true;
            }
            if (newProjectData.Comment != oldProjectData.Comment)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Comment] = " + newProjectData.Comment;
            }
            str += " WHERE ID = " + id;
            return sendInputSQLCommand(str);
        }

        //approval request
        public static async Task<List<ApprovalRequest>> GetApprovalRequestAsync()
        {
            List<ApprovalRequest> approvalsRequest = new List<ApprovalRequest>();
            String str = CreateApprovalRequestCommand();
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            foreach (Dictionary<string, object> row in rows)
            {
                try
                {
                    ApprovalRequest approvalRequest = new ApprovalRequest();
                    approvalRequest.ID = (int)(long)row["ID"];
                    approvalRequest.Approver = (row["Approver"] == DBNull.Value) ? null : (int)(long)row["Approver"];
                    approvalRequest.Approver_String = row["ApproverName"].ToString();
                    approvalRequest.LeaveRequest = (int)(long)row["Leave Request"];
                    approvalRequest.Status = Helper.ParseEnum<LeaveRequestStatus>(row["Status"].ToString());
                    approvalRequest.Comment = row["Comment"].ToString();
                    approvalsRequest.Add(approvalRequest);
                }
                catch
                {
                    // write to log about error data
                }
            }

            return approvalsRequest;
        }

        //approval request

        public static string CreateGetEmployeesCommand()
        {
            return "Select emp.*, Manager.[Full Name] as Manager from Employee as emp left join Employee as Manager on emp.[People Partner] = Manager.ID";
        }

        public static string CreateGetProjectsCommand()
        {
            return "Select pr.*, Manager.[Full Name] as Manager from Project as pr left join Employee as Manager on pr.[Project Manager] = Manager.ID";
        }

        public static string CreateApprovalRequestCommand()
        {
            return "Select ap.*, Manager.[Full Name] as ApproverName from [Approval Request] as ap left join Employee as Manager on ap.[Approver] = Manager.ID";
        }

        public static string CreateGetManagerProjectsCommand(int i)
        {
            return "Select * from Project where [Project Manager]="+i;
        }

        public static string CreateGetProjectsEmployeesCommand(int i)
        {
            return "Select * from Employee where Project=" + i;
        }

        public static string CreateGetEmployeeLeaveRequestCommand(int employeeId)
        {
            return "Select lr.*, emp.[Full Name] as EmployeeString from [Leave Request] as lr left join Employee as emp on lr.[Employee] = emp.ID where  lr.[Employee]=" + employeeId;
        }

        public static string CreateGetLeaveRequestCommand()
        {
            return "Select lr.*, emp.[Full Name] as EmployeeString from [Leave Request] as lr left join Employee as emp on lr.[Employee] = emp.ID";
        }

        public static string CreateGetMyLeaveRequestCommand(int id)
        {
            return CreateGetLeaveRequestCommand() + "where emp.ID = " + id;
        }

        public static async Task<List<LeaveRequest>> GetLeaveRequestsAsync()
        {
            List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
            String str = CreateGetLeaveRequestCommand();
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            Dictionary<string, string> displayName = await Helper.getFieldsDisplayNames(typeof(LeaveRequest));
            foreach (Dictionary<string, object> row in rows)
            {
                    LeaveRequest leaveRequest = new LeaveRequest();
                    try
                    {
                        leaveRequest.ID = (int)(long)row["ID"];
                        leaveRequest.EmployeeId = (int)(long)row["Employee"];
                        leaveRequest.Employee = row["EmployeeString"].ToString();
                        leaveRequest.Absence_Reason = row["Absence Reason"].ToString();
                        leaveRequest.Start_Date = (DateTime)row["Start Date"];
                        leaveRequest.End_Date = (DateTime)row["End Date"];
                        leaveRequest.Comment = row["Comment"].ToString();
                        leaveRequest.Status = Helper.ParseEnum<LeaveRequestStatus>(row["Status"].ToString());
                    }
                    catch
                    {
                        // write to log about error data
                    }
                    leaveRequests.Add(leaveRequest);
            }

            return leaveRequests;

        }

        public static async Task<List<LeaveRequest>> GetMyLeaveRequestsAsync(int id)
        {

            List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
            if (id == null)
                return leaveRequests;
            String str = CreateGetMyLeaveRequestCommand(id);
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            Dictionary<string, string> displayName = await Helper.getFieldsDisplayNames(typeof(LeaveRequest));
            foreach (Dictionary<string, object> row in rows)
            {
                LeaveRequest leaveRequest = new LeaveRequest();
                try
                {
                    leaveRequest.ID = (int)(long)row["ID"];
                    leaveRequest.EmployeeId = (int)(long)row["Employee"];
                    leaveRequest.Employee = row["EmployeeString"].ToString();
                    leaveRequest.Absence_Reason = row["Absence Reason"].ToString();
                    leaveRequest.Start_Date = (DateTime)row["Start Date"];
                    leaveRequest.End_Date = (DateTime)row["End Date"];
                    leaveRequest.Comment = row["Comment"].ToString();
                    leaveRequest.Status = Helper.ParseEnum<LeaveRequestStatus>(row["Status"].ToString());
                }
                catch
                {
                    // write to log about error data
                }
                leaveRequests.Add(leaveRequest);
            }

            return leaveRequests;

        }

        public static async Task<List<LeaveRequest>> GetEmployeeLeaveRequestsAsync(int empoyeeId)
        {
            List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
            String str = CreateGetEmployeeLeaveRequestCommand(empoyeeId);
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);
            Dictionary<string, string> displayName = await Helper.getFieldsDisplayNames(typeof(LeaveRequest));
            foreach (Dictionary<string, object> row in rows)
            {
                LeaveRequest leaveRequest = new LeaveRequest();
                try
                {
                    leaveRequest.ID = (int)(long)row["ID"];
                    leaveRequest.EmployeeId = (int)(long)row["Employee"];
                    leaveRequest.Employee = row["EmployeeString"].ToString();
                    leaveRequest.Absence_Reason = row["Absence Reason"].ToString();
                    leaveRequest.Start_Date = (DateTime)row["Start Date"];
                    leaveRequest.End_Date = (DateTime)row["End Date"];
                    leaveRequest.Comment = row["Comment"].ToString();
                    leaveRequest.Status = Helper.ParseEnum<LeaveRequestStatus>(row["Status"].ToString());
                }
                catch
                {
                    // write to log about error data
                }
                leaveRequests.Add(leaveRequest);
            }

            return leaveRequests;

        }

        public static async Task<LeaveRequest> GetLeaveRequestAsync(int id)
        {

            String str = CreateGetLeaveRequestCommand() + " where lr.ID=" + id;

            LeaveRequest leaveRequest = new LeaveRequest();

            List<Dictionary<string, object>> rows = sendSelectSQLCommand(str);           
            if (rows.Count == 0)
                return null;
            Dictionary<string, object> row = rows[0];
            try
            {
                leaveRequest.ID = (int)(long)row["ID"];
                leaveRequest.EmployeeId = (int)(long)row["Employee"];
                leaveRequest.Employee = row["EmployeeString"].ToString();
                leaveRequest.Absence_Reason = row["Absence Reason"].ToString();
                leaveRequest.Start_Date = (DateTime)row["Start Date"];
                leaveRequest.End_Date = (DateTime)row["End Date"];
                leaveRequest.Comment = row["Comment"].ToString();
                leaveRequest.Status = Helper.ParseEnum<LeaveRequestStatus>(row["Status"].ToString());
            }
            catch
            {
                // write to log about error data
            }
            return leaveRequest;
        }

        public static async Task<bool> Update_LeaveRequest(int id, LeaveRequest newLeaveRequest, LeaveRequest oldLeaveRequest)
        {
            bool coma = false;

            String str = "update [Leave Request] set ";
            if (newLeaveRequest.EmployeeId != oldLeaveRequest.EmployeeId)
            {

                str += "[Employee] = '" + newLeaveRequest.EmployeeId + "'";
                coma = true;
            }
            if (newLeaveRequest.Absence_Reason != oldLeaveRequest.Absence_Reason)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Absence Reason] = '" + newLeaveRequest.Absence_Reason + "'";
                coma = true;
            }

            if (newLeaveRequest.Start_Date != oldLeaveRequest.Start_Date)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Start Date] = '" + newLeaveRequest.Start_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "'";
                coma = true;
            }

            if (newLeaveRequest.End_Date != oldLeaveRequest.End_Date)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[End Date] = '" + newLeaveRequest.End_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "'";
                coma = true;
            }


            if (newLeaveRequest.Comment != oldLeaveRequest.Comment)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Comment] = '" + newLeaveRequest.Comment + "'";
                coma = true;
            }

            if (newLeaveRequest.Status != oldLeaveRequest.Status)
            {
                if (coma)
                {
                    str += ", ";
                }
                str += "[Status] = '" + newLeaveRequest.Status + "'";
                coma = true;
            }

            str += " WHERE ID = " + id;
            if (coma)
            {
                return sendInputSQLCommand(str);
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> Update_LeaveRequest_Status(LeaveRequest newLeaveRequest)
        {
            return await Update_LeaveRequest_Status_ByID(newLeaveRequest.ID, newLeaveRequest.Status);
        }

        public static async Task<bool> Update_LeaveRequest_Status_ByID(int id, LeaveRequestStatus status)
        {
            String str = "update [Leave Request] set [Status] = '" + status + "' WHERE ID = " + id;
            return sendInputSQLCommand(str);
        }

        public static async Task<bool> Add_Leave_Request(LeaveRequest newLeaveRequest)
        {
            String str = "insert into [Leave Request] ([Employee], [Absence Reason], [Start Date], [End Date], [Comment], [Status]) ";
            str += "values('" + newLeaveRequest.EmployeeId + "','" + newLeaveRequest.Absence_Reason + "','";
            str += newLeaveRequest.Start_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "','" + newLeaveRequest.End_Date.ToString("MM-dd-yyyy HH:mm:ss.fff") + "','"; 
            str += newLeaveRequest.Comment + "','" + newLeaveRequest.Status + "')";

            return sendInputSQLCommand(str);
        }
        public static async Task<bool> Add_ApprovalRequest(LeaveRequest newLeaveRequest)
        {
            String str = "insert into [Approval Request] ([Leave Request], [Status], [Comment]) ";
            str += "values('" + newLeaveRequest.ID + "','";
            str += LeaveRequestStatus.New + "','" + newLeaveRequest.Comment + "')";

            return sendInputSQLCommand(str);
        }


        //Login section
        public static async Task<Employee> Login(LoginRequest loginRequest)
        {
            string command = "Select emp.* from Employee as emp left join Users as UserData on emp.[ID] = UserData.EmployeeId where UserData.Username = '" + loginRequest.UserName + "' and UserData.Password = '" + loginRequest.Password + "'";
            Employee employee = new Employee();
            List<Dictionary<string, object>> rows = sendSelectSQLCommand(command);

            if (rows.Count == 0)
                return null;
            try
            {
                Dictionary<string, object> row = rows[0];
                employee.ID = (int)(long)row["ID"];
                employee.Full_Name = row["Full Name"].ToString();
                employee.Subdivision = row["Subdivision"].ToString();
                employee.Position = row["Position"].ToString();
                employee.Status = Helper.ParseEnum<ActiveStatus>(row["Status"].ToString());
                employee.Manager = (row["People Partner"] == DBNull.Value) ? null : (int)(long)row["People Partner"];
                employee.Manager_String = row["Manager"].ToString();
                employee.Vacation = (int)row["Out-of-Office Balance"];
                employee.Project = (row["Project"] == DBNull.Value) ? null : (int)(long)row["Project"];
            }
            catch
            {
                // write to log about error data
            }
            return employee;
        }
    }

    public class Helper
    {
        public static async Task<Dictionary<string, string>> getFieldsDisplayNames(Type t)
        {
            PropertyInfo[] MyPropertyInfo = t.GetProperties();
            DisplayNameAttribute att;
            Dictionary<string, string> DisplayNames = new Dictionary<string, string>();

            for (int i = 0; i < MyPropertyInfo.Length; i++)
            {
                att = (DisplayNameAttribute)Attribute.GetCustomAttribute(MyPropertyInfo[i], typeof(DisplayNameAttribute));
                if (att != null)
                {
                    DisplayNames.Add(MyPropertyInfo[i].Name.ToString(), att.DisplayName.ToString());
                }
            }
            return DisplayNames;
        }

        public static async Task<string> getFieldNameByDisplayNames(Type t, string name)
        {
            PropertyInfo[] MyPropertyInfo = t.GetProperties();
            DisplayNameAttribute att;
            Dictionary<string, string> DisplayNames = await getFieldsDisplayNames(t);

            string key = getFieldNameInDisplayNamesDict(DisplayNames, name);
            return key;
        }

        public static string getFieldNameInDisplayNamesDict(Dictionary<string, string> DisplayNames, string name)
        {
            string key = DisplayNames.FirstOrDefault(x => x.Value == name).Key;
            return key;
        }

        public static async Task<List<string>> getListOfFieldsWithDisplayNames(Type t)
        {
            PropertyInfo[] MyPropertyInfo = t.GetProperties();
            DisplayNameAttribute att;
            List<string> FieldsList = new List<string>();

            for (int i = 0; i < MyPropertyInfo.Length; i++)
            {
                att = (DisplayNameAttribute)Attribute.GetCustomAttribute(MyPropertyInfo[i], typeof(DisplayNameAttribute));
                if (att != null)
                {
                    FieldsList.Add(MyPropertyInfo[i].Name.ToString());
                }
            }
            return FieldsList;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

    public class LoginClass
    {

    }
}