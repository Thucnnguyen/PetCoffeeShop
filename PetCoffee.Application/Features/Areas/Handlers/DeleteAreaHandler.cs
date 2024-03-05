using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.FollowShop.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Areas.Handlers
{
    public class DeleteAreaHandler : IRequestHandler<DeleteAreaCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public DeleteAreaHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }


        public async Task<bool> Handle(DeleteAreaCommand request, CancellationToken cancellationToken)
        {
            var curAccount = await _currentAccountService.GetCurrentAccount();
            if (curAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            if (curAccount.IsVerify)
            {
                throw new ApiException(ResponseCode.AccountNotActived);
            }

            var area = (await _unitOfWork.AreaRepsitory.GetAsync(a => a.Id == request.AreaId && !a.Deleted)).FirstOrDefault();
            if (area == null)
            {
                throw new ApiException(ResponseCode.AreaNotExist);
            }
			area.DeletedAt = DateTime.UtcNow;
			await _unitOfWork.AreaRepsitory.UpdateAsync(area);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
