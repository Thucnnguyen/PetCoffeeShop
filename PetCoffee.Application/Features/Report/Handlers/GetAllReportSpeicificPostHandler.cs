using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Report.Models;
using PetCoffee.Application.Features.Report.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
namespace PetCoffee.Application.Features.Report.Handlers
{
    public class GetAllReportSpeicificPostHandler : IRequestHandler<GetAllReportSpeicificPostQuery, IList<ReportResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public GetAllReportSpeicificPostHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }

        public async Task<IList<ReportResponse>> Handle(GetAllReportSpeicificPostQuery request, CancellationToken cancellationToken)
        {
            //get Current account 
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }

            var reports = _unitOfWork.ReportRepository.Get(p => p.PostID == request.postId);

            if (reports == null)
            {
                return new List<ReportResponse>();
            }
            var response = reports.Select(report => _mapper.Map<ReportResponse>(report)).ToList();


            return response;


        }
    }
}
