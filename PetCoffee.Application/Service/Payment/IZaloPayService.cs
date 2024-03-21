

using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Service.Payment;

public interface IZaloPayService
{
	public Task<Transaction> CreatePayment(ZaloPayment payment);
}
