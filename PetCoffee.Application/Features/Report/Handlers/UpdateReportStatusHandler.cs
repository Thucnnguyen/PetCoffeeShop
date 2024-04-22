using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Report.Handlers;

public class UpdateReportStatusHandler : IRequestHandler<UpdateReportStatuscommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateReportStatusHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

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
					post.DeletedAt = DateTimeOffset.UtcNow;
					await _unitOfWork.PostRepository.UpdateAsync(post);
				}
				report.Status = ReportStatus.Accept;
				await _unitOfWork.ReportRepository.UpdateAsync(report);


				// change status for post has same postId
				var ReportByPostId = await _unitOfWork.ReportRepository.GetAsync(rp => rp.PostID == report.PostID && !rp.Deleted && rp.Status != ReportStatus.Accept);
				if (ReportByPostId.Any())
				{
					foreach (var r in ReportByPostId)
					{
						r.Status = ReportStatus.Accept;
						await _unitOfWork.ReportRepository.UpdateAsync(r);
					}
				}
			}

			if (report.CommentId != null)
			{
				var comment = await _unitOfWork.CommentRepository.Get(c => c.Id == report.CommentId && !c.Deleted)
						.FirstOrDefaultAsync();
				if (comment != null)
				{
					comment.DeletedAt = DateTimeOffset.UtcNow;
					await _unitOfWork.CommentRepository.UpdateAsync(comment);
				}
				report.Status = ReportStatus.Accept;
				await _unitOfWork.ReportRepository.UpdateAsync(report);

				// change status for post has same comment
				var ReportByCommentId = await _unitOfWork.ReportRepository.GetAsync(rp => rp.CommentId == report.CommentId && !rp.Deleted && rp.Status != ReportStatus.Accept);
				if (ReportByCommentId.Any())
				{
					foreach (var r in ReportByCommentId)
					{
						r.Status = ReportStatus.Accept;
						await _unitOfWork.ReportRepository.UpdateAsync(r);
					}
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
