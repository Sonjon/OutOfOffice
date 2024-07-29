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

namespace OutOfOffice.Components.Common
{
    public struct ActiveStatus
    {
        public const string Active = "Active";

        public const string Inactive = "Inactive";

        public static List<string> getStringList()
        {
            List<string> result = new List<string>();
            foreach (FieldInfo fieldInfo in typeof(ActiveStatus).GetFields())
            {
                result.Add(fieldInfo.Name);
            }
            return result;
        }
    }

    public struct LeaveRequestStatus
    {
        public const string New = "New";
        public const string Submitted = "Submitted";
        public const string Canceled = "Canceled";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
    }

    public struct Positions
    {
        public const string EMPLOYEE = "EMPLOYEE";

        public const string HR_MANAGER = "HR MANAGER";

        public const string PROJECT_MANAGER = "PROJECT MANAGER";

        public const string ADMINISTRATOR = "ADMINISTRATOR";

        public static List<string> getStringList()
        {
            List<string> result = new List<string>();
            foreach (FieldInfo fieldInfo in typeof(Positions).GetFields())
            {
                result.Add(fieldInfo.Name);
            }
            return result;
        }
    }

    public struct ProjectType
    {
        public const string Frontend = "Frontend";

        public const string Backend = "Backend";

        public const string Services = "Services";

        public static List<string> getStringList()
        {
            List<string> result = new List<string>();
            foreach (FieldInfo fieldInfo in typeof(ProjectType).GetFields())
            {
                result.Add(fieldInfo.Name);
            }
            return result;
        }
    }

    public struct Subdivision
    {
        public const string Div1 = "Division 1";

        public const string Div2 = "Division 2";

        public const string Div3 = "Division 3";

        public static List<string> getStringList()
        {
            List<string> result = new List<string>();
            foreach (FieldInfo fieldInfo in typeof(Subdivision).GetFields())
            {
                result.Add(fieldInfo.Name);
            }
            return result;
        }
    }
}