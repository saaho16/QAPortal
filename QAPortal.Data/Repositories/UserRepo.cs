using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Entities;

namespace QAPortal.Data.Repositories;

public interface IUserRepo : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email);
}


public class UserRepo : IUserRepo
{
    private readonly AppDbContext _context;
    public UserRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteAsync(int Id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == Id);
        if (user != null)
            _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public IQueryable<UserEntity> GetAllAsync()
    {
        return _context.Users.AsQueryable();
    }

    public Task<UserEntity?> GetByEmailAsync(string email)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<UserEntity?> GetByIdAsync(int Id)
    {
        return _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == Id);
    }

    public async Task<UserEntity?> InsertAsync(UserEntity Entity)
    {
        await _context.Users.AddAsync(Entity);

        await _context.SaveChangesAsync();

        return Entity;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> UpdateAsync(UserEntity Entity)
    {
        _context.Users.Update(Entity);

        await SaveAsync();
        return Entity;


    }
}