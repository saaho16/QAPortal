using System.Security.AccessControl;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;
using QAPortal.Data.Enums;
using QAPortal.Data.Repositories;
using QAPortal.Shared.DTOs.QADtos;

namespace QAPortal.Business.Services;

public class QuestionsService : IQuestionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public QuestionsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<QuestionDto> CreateQuestionAsync(QuestionRequestDto questionDto)
    {
        var userId = questionDto.CreatedBy;
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (userEntity == null)
        {
            throw new Exception("User not found");
        }
        if (userEntity.Role != UserRole.Admin && userEntity.Role != UserRole.User)
        {
            throw new Exception("Only Admins and Moderators can create questions");
        }

        var questionEntity = _mapper.Map<QuestionsEntity>(questionDto);
        questionEntity.CreatedAt = DateTime.Now;
        var createdQuestionEntity = await _unitOfWork.Questions.InsertAsync(questionEntity);

        var createdQuestionDto = _mapper.Map<QuestionDto>(createdQuestionEntity);

        return createdQuestionDto;

    }

    public async Task<bool> DeleteQuestionAsync(int questionId, int userId)
    {
        var questionEntity = await _unitOfWork.Questions.GetByIdAsync(questionId);
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (userEntity == null)
        {
            throw new Exception("User not found");
        }
        if (questionEntity == null)
        {
            return false;
        }

        if (userEntity.Role != UserRole.Admin && questionEntity.CreatedBy != userId)
        {
            throw new Exception("Only Admins or the creator of the question can delete the question");
        }

        await _unitOfWork.Questions.DeleteAsync(questionId);
        await _unitOfWork.Save();
        return true;

    }

    public async Task<bool> EndQuestionAsync(int questionId, int userId)
    {
        var questionEntity = await _unitOfWork.Questions.GetByIdAsync(questionId);
        var userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (questionEntity == null)
        {
            return false;
        }
        if (userEntity!.Role != UserRole.Admin)
        {
            throw new Exception("Only Admin can end the question");
        }
        questionEntity.IsEnded = true;
        await _unitOfWork.Questions.UpdateAsync(questionEntity);
        return true;

    }

    public async Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync()
    {
        var questionsEntities = _unitOfWork.Questions.GetAllAsync();
        var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questionsEntities);
        return questionDtos;
    }

    public async Task<IEnumerable<QuestionWithUserDto>> GetAllQuestionsWithUserAsync()
    {
        var questions = _unitOfWork.Questions.GetAllAsync();

        var questionsWithUser = questions
            .Include(q => q.CreatedUser)
            .Select(q => new QuestionWithUserDto
            {
                Id = q.Id,
                Title = q.Title,
                Body = q.Body,
                CreatedBy = q.CreatedBy,
                UserName = q.CreatedUser != null ? q.CreatedUser.UserName : "Unknown",
                IsEnded = q.IsEnded
            });

        return await Task.FromResult(questionsWithUser.AsEnumerable());
    }

    public async Task<QuestionDto> GetQuestionByIdAsync(int questionId)
    {
        var questionEntity = await _unitOfWork.Questions.GetByIdAsync(questionId);
        var questionDto = _mapper.Map<QuestionDto>(questionEntity);
        return questionDto;
    }

    public async Task<IEnumerable<QuestionDto>> GetQuestionsByCreatorAsync(int createdBy)
    {
        var questions = _unitOfWork.Questions.GetAllAsync();
        var questionsEntities = questions.Where(q => q.CreatedBy == createdBy);

        var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questionsEntities);
        return questionDtos;

    }

    public async Task<IEnumerable<QuestionDto>> GetQuestionsByStatusAsync(bool isEnded)
    {
        var questions = _unitOfWork.Questions.GetAllAsync();

        var questionsEntities = questions.Where(q => q.IsEnded == isEnded);

        var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questionsEntities);
        return questionDtos;
    }


    public async Task<QuestionDto> UpdateQuestionAsync(int questionId, QuestionRequestDto questionDto)
    {
        var userEntity = await _unitOfWork.Users.GetByIdAsync(questionDto.CreatedBy);
        var modifiedEntity = _mapper.Map<QuestionsEntity>(questionDto);
        modifiedEntity.Id = questionId;




        if (userEntity == null)
        {
            throw new Exception("User not found");
        }

        var modifiedByUser = await _unitOfWork.Users.GetByIdAsync(questionDto.ModifiedBy??0);

        if (modifiedByUser != null && modifiedByUser.Role != UserRole.Admin && modifiedEntity.CreatedBy != questionDto.ModifiedBy)
        {
            throw new Exception("Only Admins and Creators can modify questions");
        }

        var existingEntity = await _unitOfWork.Questions.GetByIdAsync(questionId);

        modifiedEntity.UpdatedAt = DateTime.Now;
        modifiedEntity.CreatedAt = existingEntity!.CreatedAt;
        modifiedEntity.IsEnded = existingEntity.IsEnded;

        modifiedEntity.ModifiedBy = questionDto?.ModifiedBy == 0 ? null : questionDto?.ModifiedBy;

        await _unitOfWork.Questions.UpdateAsync(modifiedEntity);

        var updatedQuestionDto = _mapper.Map<QuestionDto>(modifiedEntity);
        return updatedQuestionDto;

    }



    public async Task<IEnumerable<QuestionWithAnswersDto>> GetAllQuestionsWithAnswersAsync()
    {
        var questions = _unitOfWork.Questions.GetAllAsync();

        var questionsWithAnswers = questions.Include(q => q.Answers)
            .Select(q => new QuestionWithAnswersDto
            {
                Id = q.Id,
                Title = q.Title,
                Body = q.Body,
                CreatedBy = q.CreatedBy,
                IsEnded = q.IsEnded,
                Answers = _mapper.Map<List<AnswerDto>>(q.Answers)
            });

        return questionsWithAnswers.AsEnumerable();
    }

}