using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Handlers
{
    public class UpdateReportStatusHandler : IRequestHandler<UpdateReportStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;


        public UpdateReportStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateReportStatusCommand request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository
            .Get(predicate: p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

            if (report is null)
            {
                throw new ApiException(ResponseCode.ReportNotExist);
            }
            if (request.Status == Domain.Enums.ReportStatus.Success && report.PostID != null)
            {
                // 
                var post = await _unitOfWork.PostRepository.GetByIdAsync(report.PostID);
                if (post is not null)
                {
                    //await _unitOfWork.PostRepository.DeleteAsync(post);
                    //await _unitOfWork.SaveChangesAsync();
                    post.Status = Domain.Enums.PostStatus.Intactive;
                    post.DeletedAt = DateTime.Now;
                    await _unitOfWork.PostRepository.UpdateAsync(post);
                    await _unitOfWork.SaveChangesAsync();
                }

            }
            else if (request.Status == Domain.Enums.ReportStatus.Success && report.CommentId != null)
            {
                // 
                var comment = await _unitOfWork.CommentRepository.GetByIdAsync(report.CommentId);
                if (comment is not null)
                {
                    await _unitOfWork.CommentRepository.DeleteAsync(comment);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            report.Status = request.Status;
            await _unitOfWork.ReportRepository.UpdateAsync(report);

            // Save changes
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
