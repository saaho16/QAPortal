namespace QAPortal.Data.Entities;

public class QuestionsEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? ModifiedBy { get; set; }

    public bool? IsEnded { get; set; }

    public virtual UserEntity CreatedUser { get; set; }
    public virtual List<AnswersEntity> Answers { get; set; }
    
}