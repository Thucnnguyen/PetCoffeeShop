﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, ReservationResponse>
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;

		public ReturnOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
		}

		public async Task<ReservationResponse> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
			if (currentAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (currentAccount.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}
			if (currentAccount.IsCustomer)
			{
				var reservation = _unitOfWork.ReservationRepository.Get(p => p.Id == request.OrderId && p.CreatedById == currentAccount.Id && p.Status.Equals(OrderStatus.Success))
				.Include(r => r.Transactions)
					.ThenInclude(t => t.Wallet)
				.Include(r => r.Transactions)
					.ThenInclude(t => t.Remitter)
				.FirstOrDefault();
				if (reservation == null)
				{
					throw new ApiException(ResponseCode.ReservationNotExistOrIsRefunded);
				}
				// check time 
				if (reservation.StartTime.UtcDateTime < DateTimeOffset.UtcNow)
				{
					throw new ApiException(ResponseCode.ExpiredReservation);
				}


				decimal totalAmountRefund = 0;
				if (reservation.StartTime.UtcDateTime.AddDays(-1) > DateTimeOffset.UtcNow)
				{
					// get transaction reservation
					var transaction = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
					if (transaction == null)
					{
						throw new ApiException(ResponseCode.TransactionNotFound);
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

					reservation.Transactions.Add(newRefundTransaction);
					//return amount
					totalAmountRefund = reservation.TotalPrice;

				}
				else
				{
					if (reservation.IsTotallyRefund)
					{
						// get transaction reservation
						var transaction = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
						if (transaction == null)
						{
							throw new ApiException(ResponseCode.TransactionNotFound);
						}

						//return money for reservation
						transaction.Wallet.Balance += reservation.TotalPrice;
						transaction.Remitter.Balance -= reservation.TotalPrice;

						var newRefundTransaction = new Transaction()
						{
							WalletId = transaction.Wallet.Id,
							Amount = (decimal)reservation.TotalPrice,
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
						reservation.Transactions.Add(newRefundTransaction);
						totalAmountRefund = reservation.TotalPrice;
					}
					else
					{
						// return money back for reservation
						var transaction = reservation.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
						transaction.Wallet.Balance += (transaction.Amount * 60) / 100;
						transaction.Remitter.Balance -= (transaction.Amount * 60) / 100;

						var newRefundTransaction = new Transaction()
						{
							WalletId = transaction.Wallet.Id,
							Amount = (decimal)(reservation.TotalPrice * 60) / 100,
							Content = "Hoàn tiền đặt chỗ",
							TransactionStatus = TransactionStatus.Done,
							ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
							TransactionType = TransactionType.Refund,
						};

						reservation.Transactions.Add(newRefundTransaction);

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

						totalAmountRefund = (transaction.Amount * 60) / 100 + (transactionProduct != null ? transactionProduct.Amount : 0);
					}
				}

				reservation.Status = OrderStatus.Returned;

				var response = _mapper.Map<ReservationResponse>(reservation);
				response.AmountRefund = totalAmountRefund;



				await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
				await _unitOfWork.SaveChangesAsync();

				return response;
			}

			
			var reservationReturn = _unitOfWork.ReservationRepository.Get(p => p.Id == request.OrderId && p.Status.Equals(OrderStatus.Success))
			.Include(r => r.Transactions)
			.ThenInclude(t => t.Wallet)
			.Include(r => r.Transactions)
			.ThenInclude(t => t.Remitter)
			.FirstOrDefault();

			// return money back for reservation
			var transactionReturn = reservationReturn.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.Reserve);
			if (transactionReturn == null)
			{
				throw new ApiException(ResponseCode.TransactionNotFound);
			}


			transactionReturn.Wallet.Balance += reservationReturn.TotalPrice;
			transactionReturn.Remitter.Balance -= reservationReturn.TotalPrice;


			var newRefundTransactionByShop = new Transaction()
			{
				WalletId = transactionReturn.Wallet.Id,
				Amount = (decimal)reservationReturn.TotalPrice,
				Content = "Hoàn tiền đặt chỗ",
				TransactionStatus = TransactionStatus.Done,
				ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
				TransactionType = TransactionType.Refund,
				PetCoffeeShopId = transactionReturn.PetCoffeeShopId
			};

			reservationReturn.Transactions.Add(newRefundTransactionByShop);

			// return money back for products
			var transactionProductReturn = reservationReturn.Transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Done && t.TransactionType == TransactionType.AddProducts);
			if (transactionProductReturn != null)
			{
				transactionReturn.Wallet.Balance += transactionProductReturn.Amount;
				transactionReturn.Remitter.Balance -= transactionProductReturn.Amount;

				var newRefundTransactionProduct = new Transaction()
				{
					WalletId = transactionReturn.Wallet.Id,
					Amount = (decimal)transactionProductReturn.Amount,
					Content = "Hoàn tiền đặt Nước",
					TransactionStatus = TransactionStatus.Done,
					ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
					TransactionType = TransactionType.Refund,
				};
				reservationReturn.Transactions.Add(newRefundTransactionProduct);
			}

			var amountRefund = reservationReturn.TotalPrice;

			reservationReturn.Status = OrderStatus.Returned;

			var responseReturn = _mapper.Map<ReservationResponse>(reservationReturn);
			responseReturn.AmountRefund = amountRefund;

			await _unitOfWork.ReservationRepository.UpdateAsync(reservationReturn);
			await _unitOfWork.SaveChangesAsync();
			newRefundTransactionByShop.CreatedById = currentAccount.Id;
			await _unitOfWork.TransactionRepository.UpdateAsync(newRefundTransactionByShop);
			await _unitOfWork.SaveChangesAsync();

			return responseReturn;
		}
	}
}
