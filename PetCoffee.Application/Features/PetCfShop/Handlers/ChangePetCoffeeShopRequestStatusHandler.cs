

using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Constantsl;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class ChangePetCoffeeShopRequestStatusHandler : IRequestHandler<ChangePetCoffeeShopRequestStatusCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly INotifier _notifier;

	public ChangePetCoffeeShopRequestStatusHandler(IUnitOfWork unitOfWork, IAzureService azureService, INotifier notiier)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_notifier = notiier;
	}

	public async Task<bool> Handle(ChangePetCoffeeShopRequestStatusCommand request, CancellationToken cancellationToken)
	{
		var PetCoffShopChangeStatus = await _unitOfWork.PetCoffeeShopRepository
										.Get(shop => shop.Id == request.ShopId)
										.Include(s => s.CreatedBy)
										.FirstOrDefaultAsync();
		if (PetCoffShopChangeStatus == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		if (request.Status == ShopStatus.Active)
		{
			//set status
			PetCoffShopChangeStatus.Status = ShopStatus.Active;

			//check is request to add endtime package
			var checkIsRequest = false;
			if (PetCoffShopChangeStatus.EndTimePackage == null)
			{
				checkIsRequest = true;
				PetCoffShopChangeStatus.EndTimePackage = DateTimeOffset.UtcNow.AddMonths(3);
			}
			await _unitOfWork.PetCoffeeShopRepository.UpdateAsync(PetCoffShopChangeStatus);

			//update role cus => manager
			var createdBy = await _unitOfWork.AccountRepository.GetByIdAsync(PetCoffShopChangeStatus.CreatedById);
			if (createdBy.IsCustomer)
			{
				createdBy.Role = Role.Manager;
				await _unitOfWork.AccountRepository.UpdateAsync(createdBy);
			}

			//send email for creator request 
			if (checkIsRequest)
			{
				var EmailContent = string.Format(EmailConstant.AcceptShop, createdBy.FullName);
				await _azureService.SendEmail(createdBy.Email, EmailContent, EmailConstant.EmailSubject);
			}
			else //active shop again
			{
				var EmailContent = string.Format(EmailConstant.AcceptShop, createdBy.FullName);
				await _azureService.SendEmail(createdBy.Email, EmailContent, EmailConstant.EmailSubject);

				// active staff of shop if have
				var accountStaff = await _unitOfWork.AccountRepository
							.GetAsync(acs => acs.IsStaff && acs.Status == AccountStatus.Inactive && acs.AccountShops.Any(sId => sId.ShopId == PetCoffShopChangeStatus.Id));
				if (accountStaff != null)
				{
					accountStaff.ForEach(async s =>
					{
						s.Status = AccountStatus.Active;
						await _unitOfWork.AccountRepository.UpdateAsync(s);
					});
				}
			}

			await _unitOfWork.SaveChangesAsync();

			return true;
		}

		if (request.Status == ShopStatus.InActive)
		{
			PetCoffShopChangeStatus.Status = request.Status;
			//in active all staff
			var accountStaff = await _unitOfWork.AccountRepository
							.GetAsync(acs => acs.IsStaff && acs.Status == AccountStatus.Active && acs.AccountShops.Any(sId => sId.ShopId == PetCoffShopChangeStatus.Id));
			if (accountStaff != null)
			{
				accountStaff.ForEach(async s =>
				{
					s.Status = AccountStatus.Inactive;
					await _unitOfWork.AccountRepository.UpdateAsync(s);
				});
			}

			// return 100% order for customer
			var reservations = await _unitOfWork.ReservationRepository
				.Get(p => p.Area.PetcoffeeShopId == request.ShopId
							&& p.Status.Equals(OrderStatus.Success)
							&& p.StartTime > DateTimeOffset.UtcNow)
				.Include(r => r.Transactions)
					.ThenInclude(t => t.Wallet)
				.Include(r => r.Transactions)
					.ThenInclude(t => t.Remitter)
				.Include(r => r.Area)
				.Include(r => r.CreatedBy)
				.ToListAsync();

			foreach (var reservation in reservations)
			{
				// get transaction reservation
				var transaction = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
				if (transaction == null)
				{
					continue;
				}

				//return money for reservation
				transaction.Wallet.Balance += transaction.Amount;
				transaction.Remitter.Balance -= transaction.Amount;

				var newRefundTransaction = new Transaction()
				{
					WalletId = transaction.Wallet.Id,
					Amount = (decimal)transaction.Amount,
					Content = "Hoàn tiền đặt chỗ",
					TransactionStatus = TransactionStatus.Done,
					ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
					TransactionType = TransactionType.Refund,
				};

				//return money back for products
				var transactionProduct = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.AddProducts);
				if (transactionProduct != null)
				{
					transaction.Wallet.Balance += transactionProduct.Amount;
					transaction.Remitter.Balance -= transactionProduct.Amount;

					var newRefundTransactionProduct = new Transaction()
					{
						WalletId = transaction.Wallet.Id,
						Amount = (decimal)transaction.Amount,
						Content = "Hoàn tiền đặt Nước",
						TransactionStatus = TransactionStatus.Done,
						ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
						TransactionType = TransactionType.Refund,
					};
					reservation.Transactions.Add(newRefundTransactionProduct);
				}

				reservation.Status = OrderStatus.Returned;

			}

			await _unitOfWork.SaveChangesAsync();

			foreach (var reservation in reservations)
			{
				// Create Notification
				var notification = new Notification(
				account: reservation.CreatedBy,
				type: NotificationType.ReturnOrder,
				entityType: EntityType.Reservation,
				data: reservation);

				await _notifier.NotifyAsync(notification, true);
			}
				
			var EmailContent = string.Format(EmailConstant.InActiveShop, PetCoffShopChangeStatus.CreatedBy.FullName);
			await _azureService.SendEmail(PetCoffShopChangeStatus.CreatedBy.Email, EmailContent, EmailConstant.EmailSubject);
			return true;
		}

		if (request.Status == ShopStatus.Cancel)
		{
			PetCoffShopChangeStatus.Status = request.Status;

			await _unitOfWork.SaveChangesAsync();
			var EmailContent = string.Format(EmailConstant.RejectShop, PetCoffShopChangeStatus.CreatedBy.FullName);
			await _azureService.SendEmail(PetCoffShopChangeStatus.CreatedBy.Email, EmailContent, EmailConstant.EmailSubject);
			return true;
		}

		PetCoffShopChangeStatus.Status = request.Status;

		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
