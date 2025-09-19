using AutoMapper;
using QAPortal.Data.Entities;
using QAPortal.Data.Enums;
using QAPortal.Data.Repositories;
using QAPortal.Shared.DTOs.UserDtos;

namespace QAPortal.Business.Services;


public class ApprovalService : IApprovalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ApprovalService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ApprovalDto> ApproveUserAsync(ApprovalRequestDto approvalDto)
    {
        var approvalEntity = _mapper.Map<ApprovalEntity>(approvalDto);
        var createdApprovalEntity = await _unitOfWork.Approvals.InsertAsync(approvalEntity);
        var createdApprovalDto = _mapper.Map<ApprovalDto>(createdApprovalEntity);
        return createdApprovalDto;

    }

    public Task<List<ApprovalDto>> GetAllApprovalsAsync()
    {
        var approvalsEntities = _unitOfWork.Approvals.GetAllAsync();
        var approvalDtos = _mapper.Map<List<ApprovalDto>>(approvalsEntities);
        return Task.FromResult(approvalDtos);

    }

    public async Task<List<ApprovalDto>> GetApprovalByUserIdAsync(int userId)
    {
        var approvalEntities = _unitOfWork.Approvals.GetAllAsync().Where(a => a.UserId == userId);

        var approvalDtos = _mapper.Map<List<ApprovalDto>>(approvalEntities);
        return approvalDtos;

    }

    public async Task<List<ApprovalDto>> GetOnlylApprovedUsersAsync()
    {
        var approvalsEntities = await _unitOfWork.Approvals.GetOnlylApprovedUsersAsync();
        var approvalDtos = _mapper.Map<List<ApprovalDto>>(approvalsEntities);
        return approvalDtos;
    }

    public async Task<List<ApprovalDto>> GetOnlylPendingUsersAsync()
    {
        var approvalsEntities = _unitOfWork.Approvals.GetAllAsync().Where(a => !a.IsApproved);
        var approvalDtos = _mapper.Map<List<ApprovalDto>>(approvalsEntities);
        return approvalDtos;

    }

    public Task<bool> IsUserApprovedAsync(int userId, ApprovalFor approvalFor)
    {
        var approvalEntity = _unitOfWork.Approvals.GetAllAsync().FirstOrDefault(a => a.UserId == userId && a.ApprovalFor == approvalFor && a.IsApproved);
        if (approvalEntity == null)
        {
            return Task.FromResult(false);
        }
        if (approvalEntity.IsApproved)
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    //this has used for both accept and revoke
    public async Task<bool> ToggleApprovalAsync(int requestId, int approvedBy)
    {
        var approvalEntity = await _unitOfWork.Approvals.GetByIdAsync(requestId);
        if (approvalEntity == null)
        {
            return false;
        }
        approvalEntity.IsApproved = !approvalEntity.IsApproved;
        approvalEntity.ApprovedBy = approvedBy;

        await _unitOfWork.Approvals.UpdateAsync(approvalEntity);

        //Promote User To Admin If He's Approved For Admin Role
        if (approvalEntity.IsApproved && approvalEntity.ApprovalFor == ApprovalFor.Admin)
        {
            var userEntity = await _unitOfWork.Users.GetByIdAsync(approvalEntity.UserId);
            if (userEntity != null)
            {
                userEntity.Role = UserRole.Admin;
                await _unitOfWork.Users.UpdateAsync(userEntity);
            }
        }
        //In case Of Revoke
        else if (!approvalEntity.IsApproved && approvalEntity.ApprovalFor == ApprovalFor.Admin)
        {
            var userEntity = await _unitOfWork.Users.GetByIdAsync(approvalEntity.UserId);
            if (userEntity != null)
            {
                userEntity.Role = UserRole.User;
                await _unitOfWork.Users.UpdateAsync(userEntity);
            }
        }


        return true;

    }

    public async Task<bool> rejectApprovalAsync(int requestId)
    {
        //Modify this later to get the particular request
        var approvalEntity = await _unitOfWork.Approvals.GetByIdAsync(requestId);
        if (approvalEntity == null)
        {
            return false;
        }
        await _unitOfWork.Approvals.DeleteAsync(approvalEntity.Id);
        await _unitOfWork.Save();
        return true;

    }

    public Task<List<ApprovalWithUserDto>> GetApprovalWithUserByIdAsync(int userId)
    {
        var approvals = _unitOfWork.Approvals.GetAllAsync();
        
        var approvalEntities = approvals
            .Where(a => a.UserId == userId)
            .Select(a => new ApprovalWithUserDto
            {
                Id = a.Id,
                UserId = a.UserId,
                IsApproved = a.IsApproved,
                ApprovedBy = a.ApprovedBy,
                ApprovalFor = a.ApprovalFor,
                UserName = a.RequestedUser.UserName
            });
        return Task.FromResult(approvalEntities.ToList());



    }
}