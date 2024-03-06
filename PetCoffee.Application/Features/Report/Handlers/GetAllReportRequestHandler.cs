using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Report.Handlers
{
    public class GetAllReportRequestHandler : IRequestHandler<GetAllReportRequestQuery, PaginationResponse<Domain.Entities.Report, ReportResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllReportRequestHandler(
        IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<Domain.Entities.Report, ReportResponse>> Handle(GetAllReportRequestQuery request, CancellationToken cancellationToken)
        {
            var repoprts = await _unitOfWork.ReportRepository.GetAsync(
           predicate: request.GetExpressions(),
           disableTracking: true
       );
            var response = new List<ReportResponse>();

            return new PaginationResponse<Domain.Entities.Report, ReportResponse>(
                repoprts,
                request.PageNumber,
                request.PageSize,
                s => _mapper.Map<ReportResponse>(s));
        }
    }
}
