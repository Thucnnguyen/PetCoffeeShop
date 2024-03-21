using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Persistence.Repository;
//using System.Data.Entity;

namespace PetCoffee.Application.Features.Post.Handlers
{
    public class UpdatePostStatusHandler : IRequestHandler<UpdatePostStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;


        public UpdatePostStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePostStatusCommand request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository
            .Get(predicate: p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

            if (post is null)
            {
                throw new ApiException(ResponseCode.PostNotExisted);
            }

            post.Status = request.Status;
            await _unitOfWork.PostRepository.UpdateAsync(post);

            // Save changes
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
