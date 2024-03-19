using PetCoffee.Application.Service.Payment;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Settings;
using PetCoffee.Shared.Extensions;

namespace PetCoffee.Infrastructure.Services.Payment.ZaloPay;

public class ZaloPayService : IZaloPayService
{
	private readonly ZaloPaySettings _settings;

	public ZaloPayService(ZaloPaySettings settings)
	{
		_settings = settings;
	}

	public async Task<Transaction> CreatePayment(ZaloPayment payment)
	{
		var zalopayPayRequest = new ZaloPaymentRequest(_settings.AppId,
								_settings.AppUser,
							   DateTime.UtcNow.GetTimeStamp(),
							   (long)payment.Amount!,
							   DateTime.UtcNow.ToString("yymmdd") + "_" + payment.PaymentReferenceId ?? string.Empty,
							   "zalopayapp",
							   payment.Info ?? string.Empty,
							   _settings.RedirectUrl
							   );

		zalopayPayRequest.MakeSignature(_settings.Key1);
		(bool createZaloPayLinkResult, string? createZaloPayMessage) = zalopayPayRequest.GetLink(_settings.PaymentUrl);
		Transaction transaction = new()
		{
			Amount = payment.Amount,
			Content = payment.Info,
			ReferenceTransactionId = payment.PaymentReferenceId,
			Url = createZaloPayMessage,
		};

		return await Task.FromResult(transaction);
	}
}
