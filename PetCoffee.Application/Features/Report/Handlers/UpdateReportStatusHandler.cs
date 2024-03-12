﻿
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;
using System.Data.Entity;

namespace PetCoffee.Application.Features.Report.Handlers;

public class UpdateReportStatusHandler : IRequestHandler<UpdateReportStatuscommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public async Task<bool> Handle(UpdateReportStatuscommand request, CancellationToken cancellationToken)
	{
		var report = await _unitOfWork.ReportRepository
							.Get(r => r.Id == request.ReportId && r.Status == ReportStatus.Processing)
							.FirstOrDefaultAsync();

		if (report == null)
		{
			throw new ApiException(ResponseCode.ReportNotExisted);
		}

		if (request.Status == ReportStatus.Accept)
		{
			report.Status = ReportStatus.Accept;
			if (report.PostID != null)
			{
				var post = await _unitOfWork.PostRepository.Get(p => p.Id == report.PostID && !p.Deleted)
							.FirstOrDefaultAsync();
				if (post != null)
				{
					post.DeletedAt = DateTime.UtcNow;
					await _unitOfWork.PostRepository.UpdateAsync(post);
				}
			}

			if (report.CommentId != null)
			{
				var comments = await _unitOfWork.CommentRepository
							.GetAsync(c => c.Id == report.CommentId  && c.ParentCommentId == report.CommentId);
				if (comments != null)
				{
					await _unitOfWork.CommentRepository.DeleteRange(comments);
				}
			}


		}
		else
		{
			report.Status = request.Status;
		}

		await _unitOfWork.SaveChangesAsync();
		return true;

	}
}
