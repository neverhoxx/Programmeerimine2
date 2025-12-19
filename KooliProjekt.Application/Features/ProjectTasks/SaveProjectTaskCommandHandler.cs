using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class SaveProjectTaskCommandHandler : IRequestHandler<SaveProjectTaskCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectTaskCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectTaskCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var task = new ProjectTask();
            if (request.Id == 0)
            {
                await _dbContext.ProjectTasks.AddAsync(task, cancellationToken);
            }
            else
            {
                task = await _dbContext.ProjectTasks.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            // Ülejäänud projekti propertid ka
            task.ProjectId = request.ProjectId;
            task.UserId = request.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
