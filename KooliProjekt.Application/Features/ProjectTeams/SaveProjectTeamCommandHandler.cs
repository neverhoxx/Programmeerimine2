using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTeams
{
    public class SaveProjectTeamCommandHandler : IRequestHandler<SaveProjectTeamCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectTeamCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectTeamCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var team = new ProjectTeam();
            if (request.Id == 0)
            {
                await _dbContext.ProjectTeams.AddAsync(team, cancellationToken);
            }
            else
            {
                team = await _dbContext.ProjectTeams.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            // Ülejäänud projekti propertid ka
            team.ProjectId = request.ProjectId;
            team.UserId = request.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
