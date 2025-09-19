namespace QAPortal.Shared.DTOs.QADtos;

public class QuestionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }

    public bool? IsEnded { get; set; }
}


public class QuestionRequestDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }
}


public class QuestionWithUserDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int? CreatedBy { get; set; }
    public string UserName { get; set; }
    public bool? IsEnded { get; set; }
}

public class QuestionWithAnswersDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int? CreatedBy { get; set; }
    public bool? IsEnded { get; set; }
    public List<AnswerDto> Answers { get; set; }
}

