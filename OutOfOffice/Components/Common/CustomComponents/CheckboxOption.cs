using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Common.CustomComponents
{
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
            Text = text;
            Value = value;
        }
    }
}
