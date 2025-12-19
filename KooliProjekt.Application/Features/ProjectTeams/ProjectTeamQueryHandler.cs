using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTeams
{
    public class ProjectWLQueryHandler : IRequestHandler<ProjectTeamQuery, OperationResult<PagedResult<ProjectTeam>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectWLQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ProjectTeam>>> Handle(ProjectTeamQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<ProjectTeam>>();

            result.Value = await _dbContext
                .ProjectTeams
                .Include(pt => pt.Project)
                .Include(pt => pt.User)
                .AsNoTracking()
                .OrderBy(pt => pt.ProjectId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
