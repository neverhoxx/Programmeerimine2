using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class SaveProjectUserCommandHandler : IRequestHandler<SaveProjectUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var user = new ProjectUser();
            if (request.Id == 0)
            {
                await _dbContext.ProjectUsers.AddAsync(user, cancellationToken);
            }
            else
            {
               user = await _dbContext.ProjectUsers.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            // Ülejäänud projekti propertid ka
            user.Id = request.UserId;
            user.Id = request.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
