using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Shared.Extensions;

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

			var comment = await _unitOfWork.CommentRepository
							.Get(p => p.Id == request.CommentID)
							.FirstOrDefaultAsync();
			if (comment != null)
			{
				throw new ApiException(ResponseCode.CommentNotExist);
			}

			if (comment.CreatedById == curAccount.Id)
			{
				throw new ApiException(ResponseCode.NotReportYourself);
			}

			// check already report
			var reportComment = await _unitOfWork.ReportRepository.GetAsync(l => l.CommentId == request.CommentID && l.CreatedById == curAccount.Id);
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
