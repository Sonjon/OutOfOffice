using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Data
{
    [Table("Approval Request")]
    public class ApprovalRequestData : BaseData, IEntity
    {

        [Column("ID")]
        public long ID { get; set; }

        [Column("Approver")]
        public long? Approver { get; set; }

        [DisplayName("Approver")]
        [NotMapped]
        public string Approver_String { get; set; }

        [Column("Leave Request")]
        public long? LeaveRequestId { get; set; }

        [Column("Status")]
        [Required, DisplayName("Status")]
        public string Status { get; set; } = LeaveRequestStatus.Submitted.ToString();

        [Column("Comment")]
        [DisplayName("Comment")]
        public string? Comment { get; set; }


        [ForeignKey("LeaveRequestId")]
        public virtual LeaveRequestData? LeaveRequest { get; set; }


        [ForeignKey("Approver")]
        public virtual EmployeeData? ApproverData { get; set; }

        public void Copy(ApprovalRequestData employee)
        {
            this.ID = employee.ID;
            this.Approver = employee.Approver;
            this.Approver_String = employee.Approver_String;
            this.LeaveRequestId = employee.LeaveRequestId;
            this.Status = employee.Status;
            this.Comment = employee.Comment;
        }

        public async Task Approve(int id)
        {
            if (this.Status == LeaveRequestStatus.Submitted)
            {
                this.Status = LeaveRequestStatus.Approved;
                this.Approver = id;
                LeaveRequest.Status = LeaveRequestStatus.Approved;
            }
        }

        public async Task Reject(int id)
        {
            if (this.Status == LeaveRequestStatus.Submitted)
            {
                this.Status = LeaveRequestStatus.Rejected;
                this.Approver = id;
                LeaveRequest.Status = LeaveRequestStatus.Rejected;
            }
        }
    }
}
