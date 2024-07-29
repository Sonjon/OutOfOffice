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
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.Components.Data
{
    public enum enumActiveStatus
    {
        Active,
        Inactive
    }

    public struct ActiveStatus
    {
        public const string Active = "Active";

        public const string Inactive = "Inactive";
    }

    public enum LeaveRequestStatus
    {
        New,
        Submitted,
        Canceled,
        Approved,
        Rejected
    }

    public struct Positions
    {
        public const string EMPLOYEE = "EMPLOYEE";

        public const string HR_MANAGER = "HR MANAGER";

        public const string PROJECT_MANAGER = "PROJECT MANAGER";

        public const string ADMINISTRATOR = "ADMINISTRATOR";
    }

    public enum EmployeePositions
    {
        Employee,
        HR_Manager,
        Project_Manager,
        Administrator
    }

    public enum enumProjectType
    {
        Frontend,
        Backend,
        Services
    }

    public struct ProjectType
    {
        public const string Frontend = "Frontend";

        public const string Backend = "Backend";

        public const string Services = "Services";
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
        public long EmployeeId { get; set; }
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
