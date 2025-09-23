namespace QAPortal.Data.Repositories;

public interface IUnitOfWork
{
    //Define the Specific Repositories
    IUserRepo Users { get; }
    IAnswersRepo Answers { get; }
    IQuestionsRepo Questions { get; }

    IApprovalRepo Approvals { get; }

    void CreateTransaction();
    void Commit();
    void Rollback();
    Task Save();
}