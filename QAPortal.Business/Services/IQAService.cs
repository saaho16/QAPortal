namespace QAPortal.Business.Services;

using QAPortal.Shared.DTOs.QADtos;
using QAPortal.Data.Enums;
using QAPortal.Data.Entities;
using Microsoft.AspNetCore.Mvc;


public interface IQuestionService
{
    Task<QuestionDto> CreateQuestionAsync(QuestionRequestDto questionDto);
    Task<QuestionDto> GetQuestionByIdAsync(int questionId);
    Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync();
    Task<QuestionDto> UpdateQuestionAsync(int questionId, QuestionRequestDto questionDto);
    Task<bool> DeleteQuestionAsync(int questionId, int userId);
    Task<bool> EndQuestionAsync(int questionId, int userId);
    Task<IEnumerable<QuestionDto>> GetQuestionsByStatusAsync(bool isEnded);
    Task<IEnumerable<QuestionDto>> GetQuestionsByCreatorAsync(int createdBy);

    //Getting question with user details
    Task<IEnumerable<QuestionWithUserDto>> GetAllQuestionsWithUserAsync();

    Task<IEnumerable<QuestionWithAnswersDto>> GetAllQuestionsWithAnswersAsync();

}

public interface IAnswerService
{
    Task<AnswerDto> CreateAnswerAsync(AnswerRequestDto answerDto, int userId);
    Task<AnswerDto> GetAnswerByIdAsync(int answerId);
    Task<IEnumerable<AnswerDto>> GetAllAnswersAsync();
    Task<AnswerDto> UpdateAnswerAsync(int answerId, AnswerRequestDto answerDto);
    Task<bool> DeleteAnswerAsync(int answerId, int userId);
    Task<IEnumerable<AnswerDto>> GetAnswersByQuestionIdAsync(int questionId);
    Task<IEnumerable<AnswerDto>> GetAnswersByUserAsync(int userId);


    //Getting Answer with user details n Questions
    Task<IEnumerable<AnswersWithQuestionNUSerDto>> GetAnswerWithQuestionNUserByUserIdAsync(int userId);    
    Task<IEnumerable<AnswersWithQuestionNUSerDto>> GetAllAnswersWithQuestionNUserAsync();


}