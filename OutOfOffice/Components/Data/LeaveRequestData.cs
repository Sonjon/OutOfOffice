using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Data
{
    [Table("Leave Request")]
    public class LeaveRequestData : TableRowsBaseClass, IEntity
    {
        [Column("ID")]
        public long ID { get; set; }

        [Column("Employee")]
        public long EmployeeId { get; set; }

        [DisplayName("Employee")]
        [NotMapped]
        public string Employee { get; set; }

        [Column("Absence Reason")]
        [Required(ErrorMessage = "Please give reason for absence"), DisplayName("Absence Reason")]
        public string Absence_Reason { get; set; }

        [Column("Start Date")]
        [Required, DisplayName("Start Date")]
        public DateTime Start_Date { get; set; }

        [Column("End Date")]
        [Required, DisplayName("End Date"), DateGreaterThanAttribute(otherPropertyName = "Start_Date", ErrorMessage = "End date must be greater than start date")]
        public DateTime End_Date { get; set; }

        [Column("Comment")]
        [DisplayName("Comment")]
        public string? Comment { get; set; } 

        [Column("Status")]
        [DisplayName("Status")]
        public string Status { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeData EmployeeData { get; set; }

        public void Copy(LeaveRequestData leaveRequest)
        {
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
            if (this.Status == LeaveRequestStatus.New.ToString())
            {
                this.Status = LeaveRequestStatus.Submitted.ToString();
            }
        }

        public async void Cancel()
        {
            if (this.Status == LeaveRequestStatus.New.ToString())
            {
                this.Status = LeaveRequestStatus.Canceled.ToString();
            }
        }

    }
}
