using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;

namespace QAPortal.Data.Repositories;

public interface IApprovalRepo : IGenericRepository<ApprovalEntity>
{
    Task<ApprovalEntity?> GetByUserIdAsync(int userId);
    Task<List<ApprovalEntity>> GetOnlylApprovedUsersAsync();
}

public class ApprovalRepo : IApprovalRepo
{
    private readonly AppDbContext _context;
    public ApprovalRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteAsync(int Id)
    {
        var approval = await _context.Approvals.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
        if (approval != null)
            _context.Approvals.Remove(approval);
        await _context.SaveChangesAsync();
    }


    public IQueryable<ApprovalEntity> GetAllAsync()
    {
        return _context.Approvals.AsQueryable();
    }

    public Task<ApprovalEntity?> GetByIdAsync(int Id)
    {
        return _context.Approvals.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
    }

    public async Task<ApprovalEntity?> InsertAsync(ApprovalEntity Entity)
    {

        var userId = Entity.UserId;

        var requestingApprovalFor = Entity.ApprovalFor;

        var existingApproval = await _context.Approvals
            .FirstOrDefaultAsync(a => a.UserId == userId && a.ApprovalFor == requestingApprovalFor);
        if (existingApproval != null)
        {
            return null;
        }
        await _context.Approvals.AddAsync(Entity);
        await _context.SaveChangesAsync();
        return Entity;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<ApprovalEntity?> UpdateAsync(ApprovalEntity Entity)
    {
        _context.Approvals.Update(Entity);
        await SaveAsync();
        return Entity;
    }

    public Task<ApprovalEntity?> GetByUserIdAsync(int userId)
    {
        return _context.Approvals.FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public Task<List<ApprovalEntity>> GetOnlylApprovedUsersAsync()
    {

        return _context.Approvals.Where(a => a.IsApproved).ToListAsync();
    }
}