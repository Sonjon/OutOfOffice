using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Common;

namespace OutOfOffice.Components.Data
{
    [Table("Employee")]
    public class EmployeeData : BaseData, IEntity
    {
        [Column("ID")]
        public long ID { get; set; }

        [Column("Full Name")]
        [Required, DisplayName("Full Name")]
        public string Full_Name { get; set; }

        [Column("Subdivision")]
        [Required(ErrorMessage = "Please choose subdivision from list"), DisplayName("Subdivision")]
        public string Subdivision { get; set; }

        [Column("Position")]
        [Required(ErrorMessage = "Please choose position from list"), DisplayName("Position")]
        public string Position { get; set; }

        [Column("Status")]
        [Required, DisplayName("Status")]
        public string Status { get; set; }

        [Column("People Partner")]
        public long? Manager { get; set; }


        [DisplayName("People Partner")]
        [NotMapped]
        public string Manager_String { get; set; }

        [Column("Out-of-Office Balance")]
        [Required(ErrorMessage = "Please set your available days off"), DisplayName("Out-of-Office Balance")]
        public int Vacation { get; set; }

        [Column("Project")]
        public long? ProjectId { get; set; }


        [ForeignKey("Manager")]
        public virtual EmployeeData ManagerData { get; set; }

        [ForeignKey("ProjectId")]
        public virtual ProjectData? ProjectInformation { get; set; }


        public void Copy(EmployeeData employee)
        {
            this.ID = employee.ID;
            this.Full_Name = employee.Full_Name;
            this.Subdivision = employee.Subdivision;
            this.Position = employee.Position;
            this.Status = employee.Status;
            this.Manager = employee.Manager;
            this.Vacation = employee.Vacation;
            this.ProjectId = employee.ProjectId;
        }

        public async void Deactivate()
        {
            this.Status = ActiveStatus.Inactive;
        }

    }
}
