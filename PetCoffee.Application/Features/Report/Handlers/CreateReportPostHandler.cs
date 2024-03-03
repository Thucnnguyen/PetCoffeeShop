using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Handlers
{
    public class CreateReportPostHandler : IRequestHandler<CreateReportPostCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public CreateReportPostHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;

        }

        public async Task<bool> Handle(CreateReportPostCommand request, CancellationToken cancellationToken)
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

            var post = await _unitOfWork.PostRepository.GetAsync(p => p.Id == request.postId && p.Status == Domain.Enums.PostStatus.Active);
            if (!post.Any())
            {
                throw new ApiException(ResponseCode.PostNotExisted);
            }
            // check already report
            var reportPost = await _unitOfWork.ReportRepository.GetAsync(l => l.PostID == request.postId && l.CreatedById == curAccount.Id);
            if (reportPost.Any())
            {
                return false;
            }

            var newReportPost = _mapper.Map<Domain.Entities.Report>(request);
            newReportPost.Reason = request.ReportCategory.GetDescription();
            await _unitOfWork.ReportRepository.AddAsync(newReportPost);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
