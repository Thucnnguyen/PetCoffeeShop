﻿
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

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
			.OrderByDescending(r => r.Id)
			.AsQueryable();

		return new PaginationResponse<Domain.Entities.Report, ReportResponse>(
		reports,
		request.PageNumber,
		request.PageSize,
		r => _mapper.Map<ReportResponse>(r)
		);
	}
}
