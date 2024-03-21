using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class UpdateProductOfBookingHandler : IRequestHandler<UpdateProductOfBookingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public UpdateProductOfBookingHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
        }
        public async Task<bool> Handle(UpdateProductOfBookingCommand request, CancellationToken cancellationToken)
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

            var reservation = _unitOfWork.ReservationRepository.Get(p => p.Id == request.OrderId && p.CreatedById == currentAccount.Id && !p.Status.Equals(OrderStatus.Reject) && p.StartTime >= DateTime.Now).FirstOrDefault();
            if (reservation == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }


            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }

            //

            var existingInvoiceProduct = reservation.Invoices
        .SelectMany(i => i.Products)
        .FirstOrDefault(ip => ip.ProductId == request.ProductId);
            if (existingInvoiceProduct != null)
            {

                existingInvoiceProduct.TotalProduct += request.Quantity;
            }
            else
            {

                var newInvoiceProduct = new InvoiceProduct
                {
                    InvoiceId = reservation.Id,
                    ProductId = request.ProductId,
                    TotalProduct = request.Quantity
                };


                var firstInvoice = reservation.Invoices.FirstOrDefault();
                //if (firstInvoice != null)
                //{
                //    firstInvoice.Products.Add(newInvoiceProduct);
                //}
                //else
                //{

                var newInvoice = new Invoice
                {
                    ReservationId = reservation.Id,
                    TotalAmount = product.Price * request.Quantity,
                    Products = new List<InvoiceProduct>()
                };
                newInvoice.Products.Add(newInvoiceProduct);


                reservation.Invoices.Add(newInvoice);
                //}
            }
            //

            await _unitOfWork.SaveChangesAsync();

            return true;


        }
    }

}
