

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Report.Handlers;

public class GetReportByIdHandler : IRequestHandler<GetReportByIdQuery, ReportResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetReportByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ReportResponse> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
	{
		var Report = await _unitOfWork.ReportRepository.GetByIdAsync(request.Id);
		if (Report == null)
		{
			throw new ApiException(ResponseCode.ReportNotExisted);
		}

		return _mapper.Map<ReportResponse>(Report);	
	}
}
