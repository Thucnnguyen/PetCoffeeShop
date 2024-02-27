﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Comment.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetCoffee.Application.Features.Areas.Handlers
{
    public class GetAreaByPetCfShopIdHandler : IRequestHandler<GetAreaByPetCfShopIdQuery, PaginationResponse<Domain.Entities.Area, AreaResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;

        public GetAreaByPetCfShopIdHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
        }

        public async Task<PaginationResponse<Area, AreaResponse>> Handle(GetAreaByPetCfShopIdQuery request, CancellationToken cancellationToken)
        {
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            //if (currentAccount.IsVerify)
            //{
            //    throw new ApiException(ResponseCode.AccountNotActived);
            //}
            var CurrentShop = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(
        predicate: p => p.Id == request.ShopId,
     
        disableTracking: true
        )).FirstOrDefault();

            if (CurrentShop == null)
            {
                throw new ApiException(ResponseCode.ShopNotExisted);
            }

            var areas = _unitOfWork.AreaRepsitory.Get(c => c.PetcoffeeShopId == request.ShopId, disableTracking: true)
                                                //.Include(c => c.creaet)
                                                .ToList();

            var response = new List<AreaResponse>();

            foreach (var area in areas)
            {
                var areaResponse = _mapper.Map<AreaResponse>(area);
                //areaResponse.TotalSubComments = Comments.Count(c => c.ParentCommentId == comment.Id);
                response.Add(areaResponse);
            }

            return new PaginationResponse<Domain.Entities.Area, AreaResponse>(
       response,
       response.Count(),
       request.PageNumber,
       request.PageSize);

        }
    }
}
