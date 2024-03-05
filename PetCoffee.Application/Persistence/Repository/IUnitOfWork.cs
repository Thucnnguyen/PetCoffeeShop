

using System.Data;

namespace PetCoffee.Application.Persistence.Repository;

public interface IUnitOfWork
{
	Task<int> SaveChangesAsync();

	void Rollback();

	IDbTransaction BeginTransaction();

	IAccountRepository AccountRepository { get; }
	ICategoryRepository CategoryRepository { get; }
	ICommentRepository CommentRepository { get; }
	IMomentRepository MomentRepository { get; }
	IEventFieldRepsitory EventFieldRepsitory { get; }
	IEventRepository EventRepository { get; }
	IAreaRepsitory AreaRepsitory { get; }
	IJoinEventRepository JoinEventRepository { get; }
	IItemRepository ItemRepository { get; }
	ILikeRepository LikeRepository { get; }
	INotificationRepository NotificationRepository { get; }
	IPetCoffeeShopRepository PetCoffeeShopRepository { get; }
	IPetRepository PetRepository { get; }
	IPostCategoryRepository PostCategoryRepository { get; }
	IPostRepository PostRepository { get; }
	IReportRepository ReportRepository { get; }
	IReservationRepository ReservationRepository { get; }
	ISubmittingEventFieldRepository SubmittingEventFieldRepository { get; }
	ISubmittingEventRepository SubmittingEventRepsitory { get; }
	ITransactionRepository TransactionRepository { get; }
	IVaccinationRepository VaccinationRepository { get; }
	IWalletRepository WalletRepsitory { get; }
	IPostCoffeeShopRepository PostCoffeeShopRepository { get; }
	IFollowPetCfShopRepository FollowPetCfShopRepository { get; }
	IAccountShopRespository AccountShopRespository { get; }
}