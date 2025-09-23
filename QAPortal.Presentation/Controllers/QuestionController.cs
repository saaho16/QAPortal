using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QAPortal.Business.Services;
using QAPortal.Shared.DTOs.QADtos;
namespace User.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{

    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetQuestionById(int questionId)
    {
        var question = await _questionService.GetQuestionByIdAsync(questionId);
        if (question == null)
        {
            return NotFound();
        }
        return Ok(question);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllQuestions()
    {
        var questions = await _questionService.GetAllQuestionsAsync();
        return Ok(questions);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequestDto questionDto, [FromQuery] int userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdQuestion = await _questionService.CreateQuestionAsync(questionDto);
        return CreatedAtAction(nameof(GetQuestionById), new { questionId = createdQuestion.Id }, createdQuestion);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{questionId}")]
    public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] QuestionRequestDto questionDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedQuestion = await _questionService.UpdateQuestionAsync(questionId, questionDto);
        if (updatedQuestion == null)
        {
            return NotFound();
        }
        return Ok(updatedQuestion);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpDelete("{questionId}/{userId}")]
    public async Task<IActionResult> DeleteQuestion(int questionId, int userId)
    {
        var result = await _questionService.DeleteQuestionAsync(questionId, userId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("end/{questionId}/{userId}")]
    public async Task<IActionResult> EndQuestion(int questionId, int userId)
    {
        var result = await _questionService.EndQuestionAsync(questionId, userId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("byUserId/{createdBy}")]
    public async Task<IActionResult> GetQuestionsByCreator(int createdBy)
    {
        var questions = await _questionService.GetQuestionsByCreatorAsync(createdBy);
        return Ok(questions);
    }

    [AllowAnonymous]
    [HttpGet("questionWithUser")]
    public async Task<IActionResult> GetAllQuestionsWithUser()
    {
        var questionsWithUser = await _questionService.GetAllQuestionsWithUserAsync();
        return Ok(questionsWithUser);
    }


    [HttpGet("questionWithAnswers")]
    public async Task<IActionResult> GetAllQuestionsWithAnswers()
    {
        var questionsWithAnswers = await _questionService.GetAllQuestionsWithAnswersAsync();
        return Ok(questionsWithAnswers);
    }


}

