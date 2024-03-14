using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Tables.Models;
using PetCoffee.Application.Features.Tables.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace PetCoffee.Application.Features.Tables.Handlers
{
    public class GetTableByAreaIdHandler : IRequestHandler<GetTableByAreaIdQuery, PaginationResponse<Table, TableResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        public GetTableByAreaIdHandler(
        IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;     
        }
        public async Task<PaginationResponse<Table, TableResponse>> Handle(GetTableByAreaIdQuery request, CancellationToken cancellationToken)
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

            var shop = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(a => !a.Deleted && a.Id == request.ShopId)).FirstOrDefault();

            if (shop == null)
            {
                throw new ApiException(ResponseCode.ShopNotExisted);
            }

            var area = (await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId)).FirstOrDefault();

            if (area == null)
            {
                throw new ApiException(ResponseCode.AreaNotExist);
            }

            var tables = (await _unitOfWork.TableRepository.GetAsync(f => f.AreaId == request.AreaId)).ToList();

            var response = new List<TableResponse>();
            foreach (var i in tables)
            {
                var tableResponse = _mapper.Map<TableResponse>(i);
                response.Add(tableResponse);
            }

            return new PaginationResponse<Table, TableResponse>(
             response,
             response.Count(),
             request.PageNumber,
             request.PageSize);


        }
    }
}
