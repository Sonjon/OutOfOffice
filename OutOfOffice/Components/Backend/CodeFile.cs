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