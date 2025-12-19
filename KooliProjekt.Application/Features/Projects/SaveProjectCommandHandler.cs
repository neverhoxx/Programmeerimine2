using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class SaveProjectCommandHandler : IRequestHandler<SaveProjectCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var project = new Project();
            if(request.Id == 0)
            {
                await _dbContext.Projects.AddAsync(project, cancellationToken);
            }
            else
            {
                project = await _dbContext.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            // Ülejäänud projekti propertid ka
            project.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
