

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Payments.Commands;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Payment;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Extensions;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentResponse>
{
	private readonly IVnPayService _vnPayService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	public CreatePaymentHandler(IVnPayService vnPayService, ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_vnPayService = vnPayService;
		_currentAccountService = currentAccountService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}


	public async Task<PaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var wallet = await _unitOfWork.WalletRepsitory.Get( w => w.CreatedById == currentAccount.Id )
												.FirstOrDefaultAsync();
		long walletId = 0;
		if (wallet == null)
		{
			var newWallet = new Wallet()
			{
				CreatedById = currentAccount.Id,
			};
			await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
			await _unitOfWork.SaveChangesAsync();
			walletId = newWallet.Id;
		}

		var vnPayPayment = new VnPayPayment()
		{
			PaymentReferenceId = TokenUltils.GenerateOTPCode(8),
			Amount = (long)request.RequiredAmount,
			Info = TransactionType.TopUp.GetDescription(),
			OrderType = TransactionType.TopUp.GetDescription(),
			Time = DateTimeOffset.UtcNow
		};

		var transaction = await _vnPayService.CreatePayment(vnPayPayment);
		transaction.WalletId = wallet != null ? wallet.Id : walletId;
		await _unitOfWork.TransactionRepository.AddAsync(transaction);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<PaymentResponse>(transaction);
	}
}
