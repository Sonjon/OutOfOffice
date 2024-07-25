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
        private static SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=OutOfOffice;Integrated Security=True;");

        private static bool sendInputSQLCommand(string command)
        {
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

        public static async Task<List<ApprovalRequest>> GetApprovalRequestAsyncWithProjectManager(int id)
        {
            List<ApprovalRequest> approvalsRequest = new List<ApprovalRequest>();
            String str = CreateApprovalRequestWithProjectManagerCommand(id);
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

        public static async Task<List<ApprovalRequest>> GetApprovalRequestAsyncWithHRManager(int id)
        {
            List<ApprovalRequest> approvalsRequest = new List<ApprovalRequest>();
            String str = CreateApprovalRequestWithHRManagerCommand(id);
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


        public static async Task<List<ApprovalRequest>> GetMyApprovalRequestAsync(int id)
        {
            List<ApprovalRequest> approvalsRequest = new List<ApprovalRequest>();
            String str = CreateMyApprovalRequestCommand(id);
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


        public static string CreateApprovalRequestCommand()
        {
            return "Select ap.*, Manager.[Full Name] as ApproverName from [Approval Request] as ap left join Employee as Manager on ap.[Approver] = Manager.ID";
        }

        public static string CreateApprovalRequestWithProjectManagerCommand(int id)
        {
            return "Select ap.*, Manager.[Full Name] as ApproverName from [Approval Request] as ap left join Employee as Manager on ap.[Approver] = Manager.ID left join [Leave Request] as lr on lr.ID = ap.[Leave Request] left join Employee as emp on emp.ID = lr.Employee left join Project as pr on pr.ID = emp.Project where pr.[Project Manager] = " + id;
        }

        public static string CreateApprovalRequestWithHRManagerCommand(int id)
        {
            return "Select ap.*, Manager.[Full Name] as ApproverName from [Approval Request] as ap left join Employee as Manager on ap.[Approver] = Manager.ID left join [Leave Request] as lr on ap.[Leave Request] = lr.ID left join Employee as emp on lr.Employee = emp.ID where emp.[People Partner] = " + id;
        }

        public static string CreateMyApprovalRequestCommand(int id)
        {
            return "Select ap.*, Manager.[Full Name] as ApproverName from [Approval Request] as ap left join Employee as Manager on ap.[Approver] = Manager.ID left join [Leave Requst] as lr on ap.[Leave Request] = lr.ID where lr.Employee = " + id;
        }

        public static string CreateGetLeaveRequestCommand()
        {
            return "Select lr.*, emp.[Full Name] as EmployeeString from [Leave Request] as lr left join Employee as emp on lr.[Employee] = emp.ID";
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

        public static async Task<bool> Update_LeaveRequest_Status(LeaveRequest newLeaveRequest)
        {
            return await Update_LeaveRequest_Status_ByID(newLeaveRequest.ID, newLeaveRequest.Status);
        }

        public static async Task<bool> Update_LeaveRequest_Status_ByID(int id, LeaveRequestStatus status)
        {
            String str = "update [Leave Request] set [Status] = '" + status + "' WHERE ID = " + id;
            return sendInputSQLCommand(str);
        }

        public static async Task<bool> Add_ApprovalRequest(LeaveRequest newLeaveRequest)
        {
            String str = "insert into [Approval Request] ([Leave Request], [Status], [Comment]) ";
            str += "values('" + newLeaveRequest.ID + "','";
            str += LeaveRequestStatus.Submitted + "','" + newLeaveRequest.Comment + "')";

            return sendInputSQLCommand(str);
        }

        public static async Task<bool> Update_ApprovalRequest(ApprovalRequest approvalRequest)
        {
            String str = "update [Approval Request] set [Status] = '" + approvalRequest.Status + "', [Approver] = '"+approvalRequest.Approver+"' WHERE ID = " + approvalRequest.ID;
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

}