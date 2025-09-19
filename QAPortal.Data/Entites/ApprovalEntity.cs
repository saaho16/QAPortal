using System.ComponentModel.DataAnnotations.Schema;
using QAPortal.Data.Enums;

namespace QAPortal.Data.Entities;

public class ApprovalEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public bool IsApproved { get; set; }

    public int ApprovedBy { get; set; }

    public ApprovalFor ApprovalFor { get; set; }

    public virtual UserEntity RequestedUser { get; set; }
    
}
