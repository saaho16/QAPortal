using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace QAPortal.Data.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    public IQuestionsRepo Questions { get; }
    public IAnswersRepo Answers { get; }
    public IUserRepo Users { get; }
    public IApprovalRepo Approvals { get; }

    public UnitOfWork(AppDbContext context, IQuestionsRepo questions, IAnswersRepo answers, IUserRepo users, IApprovalRepo approvals)
    {
        _context = context;
        Questions = questions;
        Answers = answers;
        Users = users;
        Approvals = approvals;
    }
    public void CreateTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
    }

    public void Rollback()
    {
        _transaction?.Rollback();

        _transaction?.Dispose();
    }

    public async Task Save()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
