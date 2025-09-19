using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;

namespace QAPortal.Data.Repositories;

public interface IQuestionsRepo : IGenericRepository<QuestionsEntity>
{
    Task<QuestionsEntity?> GetByTitleAsync(string title);
}

public class QuestionsRepo : IQuestionsRepo
{
    private readonly AppDbContext _context;
    public QuestionsRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteAsync(int Id)
    {
         var question = await _context.Questions.AsNoTracking()
                   .FirstOrDefaultAsync(q => q.Id == Id);
        if (question != null)
            _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
      
    }


    public IQueryable<QuestionsEntity> GetAllAsync()
    {
        return _context.Questions.AsQueryable();
    }

    public Task<QuestionsEntity?> GetByIdAsync(int Id)
    {
        return _context.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id == Id);
    }

    public async Task<QuestionsEntity?> InsertAsync(QuestionsEntity Entity)
    {
        await _context.Questions.AddAsync(Entity);
        await _context.SaveChangesAsync();
        return Entity;

    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<QuestionsEntity?> UpdateAsync(QuestionsEntity Entity)
    {
        _context.Questions.Update(Entity);
        await SaveAsync();
        return Entity;
    }

    public Task<QuestionsEntity?> GetByTitleAsync(string title)
    {
        return _context.Questions.FirstOrDefaultAsync(q => q.Title == title);
    }
}