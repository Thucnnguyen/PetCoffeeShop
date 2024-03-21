
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Service.Payment;

public interface IVnPayService
{
    public Task<Transaction> CreatePayment(VnPayPayment payment);

}
