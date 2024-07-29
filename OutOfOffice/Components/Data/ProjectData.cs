using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Common;
using OutOfOffice.Components.Common.Validators;


namespace OutOfOffice.Components.Data
{
    [Table("Project")]
    public class ProjectData : BaseData, IEntity
    {
        [Column("ID")]
        public long ID { get; set; }

        [Column("Project Type")]
        [Required(ErrorMessage = "Please choose project type from list"), DisplayName("Project Type")]
        public string Project_Type { get; set; }

        [Column("Start Date")]
        [Required, DisplayName("Start Date")]
        public DateTime Start_Date { get; set; } = DateTime.Now;

        [Column("End Date")]
        [Required, DisplayName("End Date"), DateGreaterThanAttribute(otherPropertyName = "Start_Date", ErrorMessage = "End date must be greater than start date")]
        public DateTime End_Date { get; set; } = DateTime.Now;


        [Column("Project Manager")]
        [Required(ErrorMessage = "Please choose proper Project Manager"), Range(1, long.MaxValue, ErrorMessage = "Please choose Project Manager")]
        public long Manager { get; set; }

        [DisplayName("Project Manager")]
        [NotMapped]
        public string Manager_String { get; set; }


        [Column("Comment")]
        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [Column("Status")]
        [Required, DisplayName("Status")]
        public string Status { get; set; }


        [ForeignKey("Manager")]
        public virtual EmployeeData ManagerData { get; set; }


        public virtual ICollection<EmployeeData>? EmployeesList { get; set; }

        public void Copy(ProjectData employee)
        {
            this.ID = employee.ID;
            this.Project_Type = employee.Project_Type;
            this.Start_Date = employee.Start_Date;
            this.End_Date = employee.End_Date;
            this.Manager = employee.Manager;
            this.Comment = employee.Comment;
            this.Status = employee.Status;
        }


        public async void Deactivate()
        {
            this.Status = ActiveStatus.Inactive;
        }
    }
}
