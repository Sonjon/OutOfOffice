using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;


namespace OutOfOffice.Components.Data
{
    [Table("Users")]
    public class UserData : TableRowsBaseClass, IEntity
    {
        [Column("ID")]
        public long ID { get; set; }

        [Column("Username")]
        [Required]
        public string Username { get; set; }

        [Column("Password")]
        [Required]
        public string Password { get; set; }


        [Column("EmployeeId")]
        [Required]
        public long EmployeeId { get; set; }



        [ForeignKey("EmployeeId")]
        public virtual EmployeeData Employee { get; set; }
    }
}
