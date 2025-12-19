using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectWorkLogs
{
    public class DeleteProjectWLCommandHandler : IRequestHandler<DeleteProjectWLCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectWLCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProjectWLCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var workLog = await _dbContext.ProjectWorkLogs.FindAsync(new object[] { request.Id }, cancellationToken);
            if (workLog == null)
            {
                result.AddError("Project worklog not found.");
                return result;
            }

            _dbContext.ProjectWorkLogs.Remove(workLog);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
