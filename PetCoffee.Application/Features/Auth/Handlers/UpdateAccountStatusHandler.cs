

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Constantsl;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;
using System.Security.Cryptography.X509Certificates;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class UpdateAccountStatusHandler : IRequestHandler<UpdateAccountStatusCommand, bool>
{
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;

	public UpdateAccountStatusHandler(ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork, IAzureService azureService)
	{
		_currentAccountService = currentAccountService;
		_unitOfWork = unitOfWork;
		_azureService = azureService;
	}

	public async Task<bool> Handle(UpdateAccountStatusCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.Unauthorized);
		}

		if (currentAccount.IsManager)
		{
			var existedStaffAccount = await _unitOfWork.AccountRepository
				.Get(a => a.Id == request.Id && !a.Deleted)
				.Include(a => a.AccountShops)
				.FirstOrDefaultAsync();
			if (existedStaffAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}

			if (existedStaffAccount.IsManager && !currentAccount.AccountShops.Any(acs => acs.ShopId == existedStaffAccount.AccountShops.First().ShopId))
			{
				throw new ApiException(ResponseCode.PermissionDenied);

			}
			existedStaffAccount.Status = request.AccountStatus;
			await _unitOfWork.AccountRepository.UpdateAsync(existedStaffAccount);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		var existedAccount = await _unitOfWork.AccountRepository
				.Get(a => a.Id == request.Id && !a.Deleted)
				.Include(a => a.AccountShops)
				.FirstOrDefaultAsync();

		
		if (existedAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}

		if (existedAccount.IsManager)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		existedAccount.Status = request.AccountStatus;
		await _unitOfWork.AccountRepository.UpdateAsync(existedAccount);

		//if existedAccount is customer 

		if(existedAccount.IsCustomer) 
		{
			//get all reservation that not start
			var reservations = await _unitOfWork.ReservationRepository
				.Get(p => p.CreatedById == existedAccount.Id 
							&& p.Status.Equals(OrderStatus.Success)
							&& p.StartTime > DateTimeOffset.UtcNow)
				.Include(r => r.Transactions)
					.ThenInclude(t => t.Wallet)
				.Include(r => r.Transactions)
					.ThenInclude(t => t.Remitter)
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
			}
			//send email
			var EmailContent = string.Format(EmailConstant.InactiveCustomerAccount, existedAccount.FullName);
			await _azureService.SendEmail(existedAccount.Email, EmailContent, EmailConstant.InactiveCustomerAccount);
		}

		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
