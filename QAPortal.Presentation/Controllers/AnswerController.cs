using Microsoft.AspNetCore.Mvc;
using QAPortal.Business.Services;
using QAPortal.Shared.DTOs.QADtos;
namespace User.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;

    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpGet("{answerId}")]
    public async Task<IActionResult> GetAnswerById(int answerId)
    {
        var answer = await _answerService.GetAnswerByIdAsync(answerId);
        if (answer == null)
        {
            return NotFound();
        }
        return Ok(answer);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAnswers()
    {
        var answers = await _answerService.GetAllAnswersAsync();
        System.Console.WriteLine(answers);
        return Ok(answers);
    }
    [HttpPost]
    public async Task<IActionResult> CreateAnswer([FromBody] AnswerRequestDto answerDto)
    {
        System.Console.WriteLine(answerDto.CreatedBy);
        System.Console.WriteLine("Httign iy");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = answerDto.CreatedBy;

        var createdAnswer = await _answerService.CreateAnswerAsync(answerDto, userId);
        return CreatedAtAction(nameof(GetAnswerById), new { answerId = createdAnswer.Id }, createdAnswer);
    }

    [HttpPut("{answerId}")]
    public async Task<IActionResult> UpdateAnswer(int answerId, [FromBody] AnswerRequestDto answerDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedAnswer = await _answerService.UpdateAnswerAsync(answerId, answerDto);
        if (updatedAnswer == null)
        {
            return NotFound();
        }
        return Ok(updatedAnswer);
    }

    [HttpDelete("{answerId}/{userId}")]
    public async Task<IActionResult> DeleteAnswer(int answerId, int userId)
    {
        var result = await _answerService.DeleteAnswerAsync(answerId, userId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }



    [HttpGet("byQuestion/{questionId}")]
    public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
    {
        var answers = await _answerService.GetAnswersByQuestionIdAsync(questionId);
        return Ok(answers);
    }


    [HttpGet("byUser/{userId}")]
    public async Task<IActionResult> GetAnswersByUser(int userId)
    {
        var answers = await _answerService.GetAnswersByUserAsync(userId);
        return Ok(answers);
    }


    //Getting Answer with user details n Questions
    [HttpGet("withQuestionNUser")]
    public async Task<IActionResult> GetAllAnswersWithQuestionNUser()
    {
        var answers = await _answerService.GetAllAnswersWithQuestionNUserAsync();
        return Ok(answers);
    }


}

