using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Repositories;

namespace FonTech.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _context;
	private IBaseRepository<Report>? _reportRepository;
	private IBaseRepository<User>? _userRepository;
	private IBaseRepository<UserToken>? _userTokenRepository;
	private IBaseRepository<Role>? _roleRepository;

	public UnitOfWork(ApplicationDbContext context)
    {
		_context = context;
    }

	public IBaseRepository<Report> Reports => 
		_reportRepository ??= new BaseRepository<Report>(_context);
	public IBaseRepository<User> Users => 
		_userRepository ??= new BaseRepository<User>(_context);
	public IBaseRepository<UserToken> UserTokens => 
		_userTokenRepository ??= new BaseRepository<UserToken>(_context);
	public IBaseRepository<Role> Roles => 
		_roleRepository ??= new BaseRepository<Role>(_context);

	public async Task SaveChangeAsync()
	{
		await _context.SaveChangesAsync();
	}

	public void Dispose()
	{
		_context.Dispose();
	}
}
