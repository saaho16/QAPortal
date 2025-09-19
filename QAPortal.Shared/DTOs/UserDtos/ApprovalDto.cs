using QAPortal.Data.Enums;

namespace QAPortal.Shared.DTOs.UserDtos;

public class ApprovalDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public bool IsApproved { get; set; }

    public int ApprovedBy { get; set; }

    public ApprovalFor ApprovalFor { get; set; }
}

public class ApprovalRequestDto
{
    public int UserId { get; set; }

    public bool IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public ApprovalFor ApprovalFor { get; set; }
}


public class ApprovalWithUserDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public bool IsApproved { get; set; }

    public int ApprovedBy { get; set; }

    public ApprovalFor ApprovalFor { get; set; }

    public string UserName { get; set; }
}