using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;
    public UserRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext  = dbContext;
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));

    public async Task<bool> ExistActiveUserWithId(Guid userId) => await _dbContext.Users.AnyAsync(user => user.Active && user.Id.Equals(userId));
    

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email));
    }

    public async Task UpdatePassword(Guid id, string passwordHash)
    {
        await _dbContext
            .Users
            .Where(user => user.Id == id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(user => user.Password, passwordHash)); //poderia concatenar o setProperty()
    //ATENCAO: Desse jeito nao se deve usar o _dbContext.SaveChangesAsync(), pois dessa forma ele ja executa a query
    }

    public void UpdateProfile(User user)
    {
        _dbContext.Users.Attach(user); //diz para o EF "trackear" essa entidade 

        _dbContext.Entry(user).Property(user => user.Name).IsModified = true; //diz que esses 2 foram alterados na regra de neg.
        _dbContext.Entry(user).Property(user => user.Email).IsModified = true;
    }
}
