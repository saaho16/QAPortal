namespace QAPortal.Shared.DTOs.QADtos;

public class AnswerDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int QuestionId { get; set; }
    public int CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
}

public class AnswerRequestDto
{
    public string Body { get; set; }
    public int QuestionId { get; set; }
    public int CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
}

public class AnswersWithQuestionNUSerDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int? QuestionId { get; set; }
    public int? CreatedBy { get; set; }
    public string UserName { get; set; }
    public string QuestionTitle { get; set; }
}