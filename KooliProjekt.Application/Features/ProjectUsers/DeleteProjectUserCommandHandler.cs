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
    public class DeleteProjectUserCommandHandler : IRequestHandler<DeleteProjectUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProjectUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var team = await _dbContext.ProjectTeams.FindAsync(new object[] { request.Id }, cancellationToken);
            if (team == null)
            {
                result.AddError("Project team not found.");
                return result;
            }

            _dbContext.ProjectTeams.Remove(team);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
