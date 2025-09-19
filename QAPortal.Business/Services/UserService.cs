using Microsoft.AspNetCore.Mvc;
using QAPortal.Data;
using QAPortal.Shared.DTOs.UserDtos;
using QAPortal.Data.Repositories;
using AutoMapper;
using QAPortal.Data.Entities;
using QAPortal.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace QAPortal.Business.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly IApprovalService _approvalService;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IApprovalService approvalService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _approvalService = approvalService;
    }

    public async Task<bool> AuthenticateUserAsync(UserLoginDto userLoginDto)
    {
        var userEntity = await _unitOfWork.Users.GetByEmailAsync(userLoginDto.Email);
        if (userEntity == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, userEntity.PasswordHash))
        {
            return false;
        }
        return true;
    }

    public async Task<UserDto> CreateUserAsync(UserRequestDto userDto)
    {


        //Prevent USers with registered Email
        
        var userWithSameEmail = await _unitOfWork.Users.GetByEmailAsync(userDto.Email);
        if (userWithSameEmail != null)
        {
            throw new Exception("User with same email already exists 2");
        }



        var userEntity = _mapper.Map<UserEntity>(userDto);
        bool isUserRequestingForAdmin = false;
        if (userDto.Role == UserRole.Admin)
        {
            //Checking first admin
            var existingAdmins = await _unitOfWork.Users.GetAllAsync().FirstOrDefaultAsync(u => u.Role == UserRole.Admin);
            if (existingAdmins != null)
            {
                //Changing him to normal user
                userEntity.Role = UserRole.User;
                isUserRequestingForAdmin = true;
            }

        }
        userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var createdUserEntity = await _unitOfWork.Users.InsertAsync(userEntity);



        await _unitOfWork.Save();


        //raising approval for him
        // Debug why it is not working
        if (isUserRequestingForAdmin)
        {
            var approvalRequestDto = new ApprovalRequestDto
            {
                UserId = createdUserEntity!.UserId,
                ApprovedBy = null,
                IsApproved = false,
                ApprovalFor = ApprovalFor.Admin
            };

            await _approvalService.ApproveUserAsync(approvalRequestDto);
        }

        var createdUserDto = _mapper.Map<UserDto>(createdUserEntity);
        return createdUserDto;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (userEntity == null)
        {
            return false;
        }
        await _unitOfWork.Users.DeleteAsync(userId);
        return true;
    }

    public Task<List<UserDto>> GetAllUsersAsync()
    {
        var usersEntities = _unitOfWork.Users.GetAllAsync();

        var userDtos = _mapper.Map<List<UserDto>>(usersEntities);
        return Task.FromResult(userDtos);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var userEntity = await _unitOfWork.Users.GetByEmailAsync(email);
        var userDto = _mapper.Map<UserDto>(userEntity);
        return userDto;
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);

        var userDto = _mapper.Map<UserDto>(userEntity);
        return userDto;
    }

    public Task<int> GetUserIdFromTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserAdminAsync(int userId)
    {
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (userEntity == null)
        {
            return false;
        }

        return userEntity.Role == UserRole.Admin;
    }

    public async Task<bool> IsUserApprovedAsync(int userId)
    {
        // var userEntity = _unitOfWork.Users.GetByIdAsync(userId);
        // var approvalEntity = userEntity.Result?.Approval;
        // if (approvalEntity == null)
        // {
        //     return Task.FromResult(false);
        // }
        // return Task.FromResult(approvalEntity.IsApproved);
        return true;
    }

    public async Task<UserDto> UpdateUserAsync(UserDto userDto)
    {
        var existingUser = await _unitOfWork.Users.GetByIdAsync(userDto.UserId);
        var userEntity = _mapper.Map<UserEntity>(userDto);
        userEntity.UserId = userDto.UserId;
        userEntity.PasswordHash = existingUser!.PasswordHash;
        var updatedUserEntity = await _unitOfWork.Users.UpdateAsync(userEntity);
        var updatedUserDto = _mapper.Map<UserDto>(updatedUserEntity);
        return updatedUserDto;
    }
}