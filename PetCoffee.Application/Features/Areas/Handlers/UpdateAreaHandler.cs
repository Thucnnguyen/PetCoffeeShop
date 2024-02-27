using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Handlers
{
    public class UpdateAreaHandler : IRequestHandler<UpdateAreaCommand, AreaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureService _azureService;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public UpdateAreaHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _azureService = azureService;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
        }

        public async Task<AreaResponse> Handle(UpdateAreaCommand request, CancellationToken cancellationToken)
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


            var area = (await _unitOfWork.AreaRepsitory.GetAsync(
                predicate: c => c.Id == request.Id,
                includes: new List<Expression<Func<Domain.Entities.Area, object>>>()
                {
                    //c => c.CreatedBy,
                })).First();
            if (area == null)
            {
                throw new ApiException(ResponseCode.AreaNotExist);
            }

            area.Description = request.Description;
            area.TotalSeat = request.TotalSeat;
            

            if (request.Image != null)
            {
                await _azureService.CreateBlob(request.Image.FileName, request.Image);
                area.Image = await _azureService.GetBlob(request.Image.FileName);
            }
            await _unitOfWork.AreaRepsitory.UpdateAsync(area);
            await _unitOfWork.SaveChangesAsync();
            var response = _mapper.Map<AreaResponse>(area);
            
            return response;

        }
    }
}
