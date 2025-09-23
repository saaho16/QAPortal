using System.ComponentModel.DataAnnotations.Schema;

namespace QAPortal.Data.Entities;

public class AnswersEntity
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int? QuestionId { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    
    public virtual UserEntity CreatedUser { get; set; }
    public virtual QuestionsEntity Question { get; set; }
    
}