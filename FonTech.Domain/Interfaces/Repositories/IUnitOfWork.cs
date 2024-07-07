using FonTech.Domain.Entity;

namespace FonTech.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<Report> Reports { get; }

    public IBaseRepository<User> Users { get; }

    public IBaseRepository<UserToken> UserTokens { get; }

    public IBaseRepository<Role> Roles { get; }

    Task SaveChangeAsync();
}
