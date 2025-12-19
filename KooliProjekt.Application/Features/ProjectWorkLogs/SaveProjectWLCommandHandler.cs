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
    public class SaveProjectWLCommandHandler : IRequestHandler<SaveProjectWLCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectWLCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectWLCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var workLog = new ProjectWorkLog();
            if (request.Id == 0)
            {
                await _dbContext.ProjectWorkLogs.AddAsync(workLog, cancellationToken);
            }
            else
            {
                workLog = await _dbContext.ProjectWorkLogs.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            // Ülejäänud projekti propertid ka
            workLog.TaskId = request.TaskId;
            workLog.UserId = request.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
