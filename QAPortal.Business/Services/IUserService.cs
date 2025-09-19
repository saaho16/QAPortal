using QAPortal.Data.Enums;
using QAPortal.Shared.DTOs.UserDtos;

namespace QAPortal.Business.Services;

public interface IUserService
{
    Task<int> GetUserIdFromTokenAsync(string token);
    Task<bool> IsUserAdminAsync(int userId);

    Task<List<UserDto>> GetAllUsersAsync();

    Task<UserDto> GetUserByIdAsync(int userId);

    Task<UserDto> CreateUserAsync(UserRequestDto userDto);

    Task<UserDto> UpdateUserAsync(UserDto userDto);
    Task<bool> DeleteUserAsync(int userId);
    Task<bool> IsUserApprovedAsync(int userId);

    Task<bool> AuthenticateUserAsync(UserLoginDto userLoginDto);

    Task<UserDto> GetUserByEmailAsync(string email);



}

public interface IApprovalService
{
    Task<ApprovalDto> ApproveUserAsync(ApprovalRequestDto approvalDto);
    Task<List<ApprovalDto>> GetApprovalByUserIdAsync(int userId);

    Task<bool> ToggleApprovalAsync(int requestId, int approvedBy);

    Task<List<ApprovalDto>> GetAllApprovalsAsync();

    Task<List<ApprovalDto>> GetOnlylApprovedUsersAsync();

    Task<List<ApprovalDto>> GetOnlylPendingUsersAsync();

    Task<bool> IsUserApprovedAsync(int userId, ApprovalFor approvalFor);

    Task<bool> rejectApprovalAsync(int requestId);
    
    Task<List<ApprovalWithUserDto>> GetApprovalWithUserByIdAsync(int requestId);



}
