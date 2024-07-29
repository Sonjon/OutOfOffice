using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Data
{
    public class BaseData
    {
        public string GetStringValue(string name)
        {
            Type type = this.GetType();
            System.Reflection.PropertyInfo prop = type.GetProperty(name);
            var fieldValue = prop.GetValue(this, null).ToString();

            string value = (string)Convert.ChangeType(fieldValue, typeof(string));
            return value;
        }
    }

}
