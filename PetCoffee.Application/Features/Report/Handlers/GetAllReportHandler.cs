
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using static Google.Apis.Requests.BatchRequest;

namespace PetCoffee.Application.Features.Report.Handlers;

public class GetAllReportHandler : IRequestHandler<GetAllReportQuery, PaginationResponse<PetCoffee.Domain.Entities.Report, ReportResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public GetAllReportHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<Domain.Entities.Report, ReportResponse>> Handle(GetAllReportQuery request, CancellationToken cancellationToken)
	{
		var reports = await _unitOfWork.ReportRepository.GetAsync(predicate: request.GetExpressions());

		return new PaginationResponse<Domain.Entities.Report, ReportResponse>(
		reports,
		request.PageNumber,
		request.PageSize,
		r => _mapper.Map<ReportResponse>(r)
		);
	}
}
