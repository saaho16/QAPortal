using Microsoft.AspNetCore.Mvc;
using QAPortal.Business.Services;
using QAPortal.Data.Enums;
using QAPortal.Shared.DTOs.UserDtos;
namespace User.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly IApprovalService _approvalService;

    public ApprovalController(IApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

    [HttpPost]
    public async Task<IActionResult> ApproveUser([FromBody] ApprovalRequestDto approvalDto)
    {
        var result = await _approvalService.ApproveUserAsync(approvalDto);

        return Ok(result);
    }

    [HttpGet("approvals")]
    public async Task<IActionResult> GetAllApprovals()
    {
        var approvals = await _approvalService.GetAllApprovalsAsync();
        return Ok(approvals);
    }

    [HttpGet("approval/{userId}")]
    public async Task<IActionResult> GetApprovalByUserId(int userId)
    {
        var approvals = await _approvalService.GetApprovalByUserIdAsync(userId);
        if (approvals == null)
        {
            return NotFound();
        }
        return Ok(approvals);
    }

    [HttpGet("isApproved/{userId}/{approvalFor}")]
    public async Task<IActionResult> IsUserApproved(int userId, ApprovalFor approvalFor)
    {
        var isApproved = await _approvalService.IsUserApprovedAsync(userId, approvalFor);
        return Ok(isApproved);
    }


    [HttpPut("revoke/{requestId}/{approvedBy}")]
    public async Task<IActionResult> ToggleApproval(int requestId, int approvedBy)
    {
        var result = await _approvalService.ToggleApprovalAsync(requestId, approvedBy);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpGet("pendingApprovals")]
    public async Task<IActionResult> GetOnlyPendingUsers()
    {
        var approvalDtos = await _approvalService.GetOnlylPendingUsersAsync();
        return Ok(approvalDtos);
    }

    [HttpGet("approvedApprovals")]
    public async Task<IActionResult> GetOnlyApprovedUsers()
    {
        var approvalDtos = await _approvalService.GetOnlylApprovedUsersAsync();
        return Ok(approvalDtos);
    }

    [HttpDelete("reject/{requestId}")]
    public async Task<IActionResult> RejectApproval(int requestId)
    {
        var result = await _approvalService.rejectApprovalAsync(requestId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpGet("withUser/{userId}")]
    public async Task<IActionResult> GetApprovalWithUserById(int userId)
    {
        var approvals = await _approvalService.GetApprovalWithUserByIdAsync(userId);
        if (approvals == null)
        {
            return NotFound();
        }
        return Ok(approvals);
    }

}

