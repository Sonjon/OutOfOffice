using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using OutOfOffice.Components.Backend;
using System.ComponentModel;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Data
{
    public enum ActiveStatus
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
        [Required(ErrorMessage = "Please choose subdivision from list"), DisplayName("Subdivision")]
        public string Subdivision { get; set; }
        [Required(ErrorMessage = "Please choose position from list"), DisplayName("Position")]
        public string Position { get; set; }
        [Required, DisplayName("Status")]
        public ActiveStatus Status { get; set; }

        public int? Manager { get; set; }
        [DisplayName("People Partner")]
        public string Manager_String { get; set; }

        [Required(ErrorMessage = "Please set your available days off"), DisplayName("Out-of-Office Balance")]
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
            this.Status = ActiveStatus.Inactive;
        }
    }

    public class LeaveRequest : TableRowsBaseClass
    {
        public int ID;
        public int EmployeeId { get; set; }

        [DisplayName("Employee")]
        public string Employee { get; set; }

        [Required(ErrorMessage = "Please give reason for absence"), DisplayName("Absence Reason")]
        public string Absence_Reason { get; set; }

        [Required, DisplayName("Start Date")]
        public DateTime Start_Date { get; set; }

        [Required, DisplayName("End Date"), DateGreaterThanAttribute(otherPropertyName = "Start_Date", ErrorMessage = "End date must be greater than start date")]
        public DateTime End_Date { get; set; }

        [Required, DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Status")]
        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.New;

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

        public async void Submitt()
        {
            if (this.Status == LeaveRequestStatus.New)
            {
                this.Status = LeaveRequestStatus.Submitted;
                await Backend.Backend.Update_LeaveRequest_Status(this);
                await Backend.Backend.Add_ApprovalRequest(this);
            }
        }

        public async void Cancel()
        {
            if (this.Status == LeaveRequestStatus.New)
            {
                this.Status = LeaveRequestStatus.Canceled;
                await Backend.Backend.Update_LeaveRequest_Status(this);
            }
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

    public class ApprovalRequest : TableRowsBaseClass
    {
        public int ID;
        public int? Approver { get; set; }

        [Required, DisplayName("Approver")]
        public string Approver_String { get; set; }

        public int? LeaveRequest { get; set; }

        [Required, DisplayName("Status")]
        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Submitted;

        [Required, DisplayName("Comment")]
        public string Comment { get; set; }

        public void Copy(ApprovalRequest employee)
        {
            this.ID = employee.ID;
            this.Approver = employee.Approver;
            this.Approver_String = employee.Approver_String;
            this.LeaveRequest = employee.LeaveRequest;
            this.Status = employee.Status;
            this.Comment = employee.Comment;
        }

        public async void Approve(int id)
        {
            if (this.Status == LeaveRequestStatus.Submitted)
            {
                this.Status = LeaveRequestStatus.Approved;
                this.Approver = id;
                await Backend.Backend.Update_LeaveRequest_Status_ByID((int)this.LeaveRequest, LeaveRequestStatus.Approved);
                await Backend.Backend.Update_ApprovalRequest(this);
            }
        }

        public async void Reject(int id)
        {
            if (this.Status == LeaveRequestStatus.Submitted)
            {
                this.Status = LeaveRequestStatus.Rejected;
                this.Approver = id;
                await Backend.Backend.Update_LeaveRequest_Status_ByID((int)this.LeaveRequest, LeaveRequestStatus.Rejected);
                await Backend.Backend.Update_ApprovalRequest(this);
            }
        }
    }

    public class checkboxOption
    {
        public int id;
        [Required]
        public string Text { get; set; } = "";
        [Required]
        public bool Value { get; set; } = false;
        public checkboxOption() { }
        public checkboxOption(string text, bool value)
        {
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
            public const string EMPLOYEE_ID = "EMPLOYEE_ID";
        }

        public struct Roles
        {
            public const string EMPLOYEE = "EMPLOYEE";
            public const string HR_MANAGER = "HR_MANAGER";
            public const string PROJECT_MANAGER = "PROJECT_MANAGER";
            public const string ADMINISTRATOR = "ADMINISTRATOR";
        }

        public string UserName { get; set; }
        public int EmployeeId { get; set; }
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

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        public string otherPropertyName;
        public DateGreaterThanAttribute() { }
        public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                if (field == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));

                if ((field.PropertyType == typeof(DateTime) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);

                    if (toValidate.CompareTo(referenceProperty) < 1)
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validationResult;
        }
    }
}
