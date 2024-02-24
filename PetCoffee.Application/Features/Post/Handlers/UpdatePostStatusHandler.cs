using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new ApiException(ResponseCode.PostNotExist);
            }



            post.Status = request.Status;
            await _unitOfWork.PostRepository.UpdateAsync(post);

            // Save changes
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
