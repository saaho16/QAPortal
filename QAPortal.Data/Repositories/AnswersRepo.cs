using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;

namespace QAPortal.Data.Repositories;

public interface IAnswersRepo : IGenericRepository<AnswersEntity>
{
    Task<AnswersEntity?> GetByContentAsync(string content);
}

public class AnswersRepo : IAnswersRepo
{
    private readonly AppDbContext _context;
    public AnswersRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteAsync(int Id)
    {
        var answer = await _context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
        if (answer != null)
            _context.Answers.Remove(answer);

        await _context.SaveChangesAsync();
    }


    public IQueryable<AnswersEntity> GetAllAsync()
    {
        return _context.Answers.AsQueryable();
    }

    public Task<AnswersEntity?> GetByIdAsync(int Id)
    {
        return _context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
    }

    public async Task<AnswersEntity?> InsertAsync(AnswersEntity Entity)
    {
        await _context.Answers.AddAsync(Entity);
        await _context.SaveChangesAsync();
        return Entity;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<AnswersEntity?> UpdateAsync(AnswersEntity Entity)
    {
        _context.Answers.Update(Entity);
        await SaveAsync();
        return Entity;
    }

    public Task<AnswersEntity?> GetByContentAsync(string content)
    {
        return _context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.Body == content);
    }
}