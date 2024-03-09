using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.Payments.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Enums;
using Microsoft.Extensions.Logging;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class ProcessVnpayPaymentIpnHandler : IRequestHandler<ProcessVnpayPaymentIpnCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ILogger<ProcessVnpayPaymentIpnHandler> _logger;

	public ProcessVnpayPaymentIpnHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProcessVnpayPaymentIpnHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task Handle(ProcessVnpayPaymentIpnCommand request, CancellationToken cancellationToken)
	{
		var resultData = new Transaction();

		var transaction = await _unitOfWork.TransactionRepository
								.Get(t => t.ReferenceTransactionId == request.ReferenceId && t.TransactionStatus == TransactionStatus.Processing && t.TransactionType == TransactionType.TopUp)
								.FirstOrDefaultAsync();

		if (transaction == null)
		{
			throw new ApiException(ResponseCode.TransactionNotFound);
		}

		if (request.Vnp_ResponseCode != "00")
		{
			_logger.LogInformation($"{request.ReferenceId} không thành công");
			transaction.TransactionStatus = TransactionStatus.Cancel;
			await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
			await _unitOfWork.SaveChangesAsync();
			return;
		}

		transaction.TransactionStatus = TransactionStatus.Done;
		await _unitOfWork.TransactionRepository.UpdateAsync(transaction);	
		var wallet = await _unitOfWork.WalletRepsitory.Get(w => w.CreatedById == transaction.CreatedById).FirstOrDefaultAsync();
		if (wallet == null)
		{
			var newWallet = new Wallet()
			{
				CreatedById = transaction.CreatedById,
				Balance = transaction.Amount,
			};
			await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation($"Tạo ví mới {newWallet.CreatedById} với số tiền {newWallet.Balance}");
			return;
		}
		wallet.Balance += transaction.Amount;
		await _unitOfWork.WalletRepsitory.UpdateAsync(wallet);
		await _unitOfWork.SaveChangesAsync();
		_logger.LogInformation($" ví {wallet.CreatedById} được nạp thêm số tiền {transaction.Amount}");
		return;
	}
}
