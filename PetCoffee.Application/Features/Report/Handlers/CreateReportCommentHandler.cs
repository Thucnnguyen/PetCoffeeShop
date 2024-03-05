using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Handlers
{
    public class CreateReportCommentHandler : IRequestHandler<CreateReportCommentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public CreateReportCommentHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;

        }
        public async Task<bool> Handle(CreateReportCommentCommand request, CancellationToken cancellationToken)
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

            var comment = await _unitOfWork.CommentRepository.GetAsync(p => p.Id == request.Id);
            if (!comment.Any())
            {
                throw new ApiException(ResponseCode.CommentNotExist);
            }
            // check already report
            var reportComment = await _unitOfWork.ReportRepository.GetAsync(l => l.CommentId == request.Id && l.CreatedById == curAccount.Id);
            if (reportComment.Any())
            {
                return false;
            }

            var newReportComment = _mapper.Map<Domain.Entities.Report>(request);
            newReportComment.Reason = request.ReportCategory.GetDescription();
            await _unitOfWork.ReportRepository.AddAsync(newReportComment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
