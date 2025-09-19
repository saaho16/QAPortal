namespace QAPortal.Data.Repositories;

public interface IUnitOfWork
{
    //Define the Specific Repositories
    UserRepo Users { get; }
    AnswersRepo Answers { get; }
    QuestionsRepo Questions { get; }

    ApprovalRepo Approvals { get; }

    //This Method will Start the database Transaction
    void CreateTransaction();

    //This Method will Commit the database Transaction
    void Commit();

    //This Method will Rollback the database Transaction
    void Rollback();

    //This Method will call the SaveChanges method
    Task Save();
}