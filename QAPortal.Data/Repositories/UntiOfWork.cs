using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace QAPortal.Data.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext Context;
    private IDbContextTransaction? _objTran = null;

    public UserRepo Users { get; private set; }


    public AnswersRepo Answers { get; private set; }

    public QuestionsRepo Questions { get; private set; }


    public ApprovalRepo Approvals { get; private set; }


    public UnitOfWork(AppDbContext _Context)
    {
        Context = _Context;
        Users = new UserRepo(Context);
        Answers = new AnswersRepo(Context);
        Questions = new QuestionsRepo(Context);
        Approvals = new ApprovalRepo(Context);
    }
    public void CreateTransaction()
    {
        _objTran = Context.Database.BeginTransaction();
    }

    public void Commit()
    {
        _objTran?.Commit();
    }

    public void Rollback()
    {
        _objTran?.Rollback();

        _objTran?.Dispose();
    }

    public async Task Save()
    {
        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
