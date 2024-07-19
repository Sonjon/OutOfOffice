using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using OutOfOffice.Components.Backend;
using System.ComponentModel;
using System.Reflection.Metadata;

namespace OutOfOffice.Components.Data
{
    public enum EmploymentStatus
    {
        Active,
        Inactive
    }

    public enum LeaveRequestStatus
    {
        New,
        Submitted,
        Canceled,
        Approved,
        Rejected
    }

    public enum EmployeePositions
    {
        Employee,
        HR_Manager,
        Project_Manager,
        Administrator
    }

    public enum ProjectType
    {
        Frontend,
        Backend,
        Services
    }

    //Approve/Reject/New

    public class TableRowsBaseClass
    {
        /*
         * Returns value of property as a string
         * 
         * name - name of value
         */
        public string GetStringValue(string name)
        {
            Type type = this.GetType();
            System.Reflection.PropertyInfo prop = type.GetProperty(name);
            var fieldValue = prop.GetValue(this, null).ToString();

            string value = (string)Convert.ChangeType(fieldValue, typeof(string));
            return value;
        }
    }

    public class Employee : TableRowsBaseClass
    {
        public int ID;
        [Required, DisplayName("Full Name")]
        public string Full_Name { get; set; }
        [Required, DisplayName("Subdivision")]
        public string Subdivision { get; set; }
        [Required, DisplayName("Position")]
        public string Position { get; set; }
        [Required, DisplayName("Status")]
        public EmploymentStatus Status { get; set; }

        public int? Manager { get; set; }
        [DisplayName("People Partner")]
        public string Manager_String { get; set; }

        [DisplayName("Out-of-Office Balance")]
        public int Vacation { get; set; }


        public int? Project { get; set; }

        public void Copy(Employee employee)
        {
            this.ID = employee.ID;
            this.Full_Name = employee.Full_Name;
            this.Subdivision = employee.Subdivision;
            this.Position = employee.Position;
            this.Status = employee.Status;
            this.Manager = employee.Manager;
            this.Manager_String = employee.Manager_String;
            this.Vacation = employee.Vacation;
            this.Project = employee.Project;
        }
        
        public async void Deactivate()
        {
            this.Status = EmploymentStatus.Inactive;
        }
    }

    public class LeaveRequest : TableRowsBaseClass
    {
        public int ID;
        public int? EmployeeId { get; set; }

        [DisplayName("Employee")]
        public string Employee { get; set; }

        [Required, DisplayName("Absence Reason")]
        public string Absence_Reason { get; set; }

        [Required, DisplayName("Start Date")]
        public DateTime Start_Date { get; set; }

        [Required, DisplayName("End Date")]
        public DateTime End_Date { get; set; }

        [Required, DisplayName("Comment")]
        public string Comment { get; set; }

        [Required, DisplayName("Status")]
        public LeaveRequestStatus Status { get; set; }

        public void Copy(LeaveRequest leaveRequest)
        {
            this.ID = leaveRequest.ID;
            this.EmployeeId = leaveRequest.EmployeeId;
            this.Employee = leaveRequest.Employee;
            this.Absence_Reason = leaveRequest.Absence_Reason;
            this.Start_Date = leaveRequest.Start_Date;
            this.End_Date = leaveRequest.End_Date;
            this.Comment = leaveRequest.Comment;
            this.Status = leaveRequest.Status;
        }
    }

    public class Project : TableRowsBaseClass
    {
        public int ID;
        [Required(ErrorMessage = "Please choose project type from list"), DisplayName("Project Type")]
        public string Project_Type { get; set; }

        [Required, DisplayName("Start Date")]
        public DateTime Start_Date { get; set; } = DateTime.Now;

        [Required, DisplayName("End Date"), DateGreaterThanAttribute(otherPropertyName = "Start_Date", ErrorMessage = "End date must be greater than start date")]
        public DateTime End_Date { get; set; } = DateTime.Now;

        public int? Manager { get; set; }
        [DisplayName("Project Manager")]
        public string Manager_String { get; set; }


        [DisplayName("Comment")]
        public string Comment { get; set; }

        [Required, DisplayName("Status")]
        public ActiveStatus Status { get; set; }

        public void Copy(Project employee)
        {
            this.ID = employee.ID;
            this.Project_Type = employee.Project_Type;
            this.Start_Date = employee.Start_Date;
            this.End_Date = employee.End_Date;
            this.Manager = employee.Manager;
            this.Manager_String = employee.Manager_String;
            this.Comment = employee.Comment;
            this.Status = employee.Status;
        }


        public async void Deactivate()
        {
            this.Status = ActiveStatus.Inactive;
        }
    }

    public class checkboxOption
    {
        public int id;
        [Required]
        public string Text { get; set; } = "";
        [Required]
        public bool Value { get; set; } = false;
        public checkboxOption() {}
        public checkboxOption(string text, bool value) {
            this.Text = text;
            this.Value = value;
        }
    }
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserAuthData
    {
        public readonly struct AuthKeys
        {
            public const string USER_NAME = "USER_NAME";
            public const string ROLE = "ROLE";
            public const string IS_LOGGED = "IS_LOGGED";
        }

        public struct Roles
        {
            public const string EMPLOYEE = "EMPLOYEE";
            public const string HR_MANAGER = "HR_MANAGER";
            public const string PROJECT_MANAGER = "PROJECT_MANAGER";
            public const string ADMINISTRATOR = "ADMINISTRATOR";
        }

        public string UserName { get; set; }
        public string Role { get; set; }
        public List<Claim> Claims { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Claim[] claims = Claims.ToArray();
            ClaimsIdentity identity = new ClaimsIdentity(claims, "WebAppAuth");

            claimsPrincipal.AddIdentity(identity);

            return claimsPrincipal;
        }

    }

    public class Person
    {
        [Required]
        public string Name { get; set; }
        [Range(18, 80, ErrorMessage = "Age must be between 18 and 80.")]
        public int Age { get; set; }
    }
}
