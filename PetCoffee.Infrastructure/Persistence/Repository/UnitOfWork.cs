
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Infrastructure.Persistence.Context;
using System.Data;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	private bool _disposed;
	private readonly IServiceScopeFactory _scopeFactory;



	public UnitOfWork(ApplicationDbContext dbContext, IServiceScopeFactory scopeFactory)
	{
		_dbContext = dbContext;
		_scopeFactory = scopeFactory;
	}

	public IDbTransaction BeginTransaction()
	{
		var transaction = _dbContext.Database.BeginTransaction();
		return transaction.GetDbTransaction();
	}

	public void Rollback()
	{
		foreach (var entry in _dbContext.ChangeTracker.Entries())
			switch (entry.State)
			{
				case EntityState.Added:
					entry.State = EntityState.Detached;
					break;
			}
	}

	public async Task<int> SaveChangesAsync()
	{
		return await _dbContext.SaveChangesAsync();
	}

	protected virtual void Dispose()
	{
		_dbContext.Dispose();
	}

	private IAccountRepository? _accountRepository;
	private ICategoryRepository? _categoryRepository;
	private ICommentRepository? _commentRepository;
	private IMomentRepository? _momentRepository;
	private IEventFieldRepsitory? _eventFieldRepsitory;
	private IEventRepository? _eventRepository;
	private IAreaRepsitory? _floorRepository;
	private IFollowEventRepository _followEventRepository;
	private IItemRepository? _itemRepository;
	private ILikeRepository? _likeRepository;
	private INotificationRepository? _notificationRepository;	
	private IPetCoffeeShopRepository? _petCoffeeShopRepository;
	private IPetRepository? _petRepository;
	private IPostCategoryRepository? _postCategoryRepository;
	private IPostRepository? _postRepository;
	private IReportRepository? _reportRepository;
	private IReservationRepository? _reservationRepository;
	private ISubmittingEventFieldRepository? _submittingEventFieldRepository;
	private ISubmittingEventRepository? _submittingEventRepsitory;
	private ITransactionRepository? _transactionRepository;
	private IVaccinationRepository? _vaccinationRepository;
	private IWalletRepository? _walletRepsitory;
	private IPostCoffeeShopRepository? _postCoffeeShopRepository;

	public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_dbContext);
	public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_dbContext);
	public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_dbContext);
	public IMomentRepository MomentRepository => _momentRepository ??= new MomentRepository(_dbContext);
	public IEventFieldRepsitory EventFieldRepsitory => _eventFieldRepsitory ??= new EventFieldRepsitory(_dbContext);
	public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_dbContext);
	public IAreaRepsitory FloorRepository => _floorRepository ??= new AreaRepository(_dbContext);
	public IFollowEventRepository FollowEventRepository => _followEventRepository ??= new FollowEventRepository(_dbContext);
	public IItemRepository ItemRepository => _itemRepository ??= new ItemRepository(_dbContext);
	public ILikeRepository LikeRepository => _likeRepository ??= new LikeRepository(_dbContext);
	public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_dbContext);
	public IPetCoffeeShopRepository PetCoffeeShopRepository => _petCoffeeShopRepository ??= new PetCoffeeShopRepository(_dbContext);
	public IPetRepository PetRepository => _petRepository ??= new PetRepository(_dbContext);
	public IPostCategoryRepository PostCategoryRepository => _postCategoryRepository ??= new PostCategoryRepository(_dbContext);
	public IPostRepository PostRepository => _postRepository ??= new PostRepository(_dbContext);
	public IReportRepository ReportRepository => _reportRepository ??= new ReportRepository(_dbContext);
	public IReservationRepository ReservationRepository => _reservationRepository ??= new ReservationRepository(_dbContext);
	public ISubmittingEventFieldRepository SubmittingEventFieldRepository => _submittingEventFieldRepository ??= new SubmittingEventFieldRepository(_dbContext);
	public ISubmittingEventRepository SubmittingEventRepsitory => _submittingEventRepsitory ??= new SubmittingEventRepository(_dbContext);
	public ITransactionRepository TransactionRepository => _transactionRepository ??= new TransactionRepository(_dbContext);
	public IVaccinationRepository VaccinationRepository => _vaccinationRepository ??= new VaccinationRepository(_dbContext);
	public IWalletRepository WalletRepsitory => _walletRepsitory ??= new WalletRepository(_dbContext);
	public IPostCoffeeShopRepository PostCoffeeShopRepository => _postCoffeeShopRepository ??= new PostPetCoffeeShopRepository(_dbContext);
}
