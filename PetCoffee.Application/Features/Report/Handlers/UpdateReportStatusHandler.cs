using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Report.Commands;
using PetCoffee.Application.Persistence.Repository;
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

            report.Status = request.Status;
            await _unitOfWork.ReportRepository.UpdateAsync(report);

            // Save changes
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
