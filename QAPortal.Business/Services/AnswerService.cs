using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;
using QAPortal.Data.Enums;
using QAPortal.Data.Repositories;
using QAPortal.Shared.DTOs.QADtos;

namespace QAPortal.Business.Services;

public class AnswerService : IAnswerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AnswerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<AnswerDto> CreateAnswerAsync(AnswerRequestDto answerDto, int userId)
    {
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);

        if (userEntity == null)
        {
            throw new Exception("User not found");
        }

        var answerEntity = _mapper.Map<AnswersEntity>(answerDto);
        answerEntity.CreatedAt = DateTime.Now;
        var createdAnswerEntity = await _unitOfWork.Answers.InsertAsync(answerEntity);
        var createdAnswerDto = _mapper.Map<AnswerDto>(createdAnswerEntity);
        return createdAnswerDto;

    }

    public async Task<bool> DeleteAnswerAsync(int answerId, int userId)
    {
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (userEntity == null)
        {
            throw new Exception("User not found");
        }
        var answerEntity = await _unitOfWork.Answers.GetByIdAsync(answerId);

        if (answerEntity == null)
        {
            return false;
        }
        if (userEntity.Role != UserRole.Admin && answerEntity.CreatedBy != userId)
        {
            throw new Exception("Only Admins or the creator of the answer can delete the answer");
        }

        await _unitOfWork.Answers.DeleteAsync(answerId);
        return true;

    }

    public Task<IEnumerable<AnswerDto>> GetAllAnswersAsync()
    {
        var answersEntities = _unitOfWork.Answers.GetAllAsync();
        var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(answersEntities);
        return Task.FromResult(answerDtos);
    }

    public async Task<AnswerDto> GetAnswerByIdAsync(int answerId)
    {
        var answerEntity = await _unitOfWork.Answers.GetByIdAsync(answerId);
        var answerDto = _mapper.Map<AnswerDto>(answerEntity);
        return answerDto;
    }

    public Task<IEnumerable<AnswerDto>> GetAnswersByQuestionIdAsync(int questionId)
    {
        var answersEntities = _unitOfWork.Answers.GetAllAsync().Where(a => a.QuestionId == questionId);
        var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(answersEntities);

        return Task.FromResult(answerDtos);

    }

    public async Task<IEnumerable<AnswerDto>> GetAnswersByUserAsync(int userId)
    {
        var answersEntities = _unitOfWork.Answers.GetAllAsync().Where(a => a.CreatedBy == userId);
        var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(answersEntities);

        return answerDtos;
    }

    public async Task<AnswerDto> UpdateAnswerAsync(int answerId, AnswerRequestDto answerDto)
    {
        var answerEntity = await _unitOfWork.Answers.GetByIdAsync(answerId);
        answerEntity!.Body = answerDto.Body;


        var modifiedByUser = await _unitOfWork.Users.GetByIdAsync(answerDto.ModifiedBy ?? 0);
        if (modifiedByUser != null && modifiedByUser.Role != UserRole.Admin && answerEntity.CreatedBy!= answerDto.ModifiedBy)
        {
            throw new Exception("Only Admins And Creators can modify Answers");
        }

        answerEntity.UpdatedAt = DateTime.Now;
        answerEntity.ModifiedBy = answerDto?.ModifiedBy == 0 ? null : answerDto?.ModifiedBy;

        await _unitOfWork.Answers.UpdateAsync(answerEntity);
        var updatedAnswerDto = _mapper.Map<AnswerDto>(answerEntity);
        return updatedAnswerDto;

    }

    public async Task<IEnumerable<AnswersWithQuestionNUSerDto>> GetAllAnswersWithQuestionNUserAsync()
    {
        var answers = _unitOfWork.Answers.GetAllAsync();
        var result = answers.Include(a => a.CreatedUser)
            .Include(a => a.Question)
            .Select(a => new AnswersWithQuestionNUSerDto
            {
                Id = a.Id,
                Body = a.Body,
                QuestionId = a.QuestionId,
                CreatedBy = a.CreatedBy,
                UserName = a.CreatedUser != null ? a.CreatedUser.UserName : "Unknown",
                QuestionTitle = a.Question != null ? a.Question.Title : "Unknown"
            }).AsEnumerable();


        return result;
    }
}