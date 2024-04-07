
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Report.Handlers;

public class GetReportByShopIdHandler : IRequestHandler<GetReportByShopIdQuery, PaginationResponse<PetCoffee.Domain.Entities.Report, ReportResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetReportByShopIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PaginationResponse<Domain.Entities.Report, ReportResponse>> Handle(GetReportByShopIdQuery request, CancellationToken cancellationToken)
	{
		var reports = _unitOfWork.ReportRepository
			.Get(
				predicate: request.GetExpressions()
			)
			.Include(r => r.CreatedBy)
			.Include(r => r.Comment)
				.ThenInclude(c => c.CreatedBy)
			.Include(r => r.Comment)
				.ThenInclude(c => c.PetCoffeeShop)
			.Include(r => r.Post)
				.ThenInclude(p => p.CreatedBy)
			.Include(r => r.Post)
				.ThenInclude(p => p.PetCoffeeShop)
			.AsQueryable();

		return new PaginationResponse<Domain.Entities.Report, ReportResponse>(
		reports,
		request.PageNumber,
		request.PageSize,
		r => _mapper.Map<ReportResponse>(r)
		);
	}
}
