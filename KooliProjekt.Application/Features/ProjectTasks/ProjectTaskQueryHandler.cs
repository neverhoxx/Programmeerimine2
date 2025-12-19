using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectTeams;
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

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class ProjectTaskQueryHandler : IRequestHandler<ProjectTaskQuery, OperationResult<PagedResult<ProjectTask>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectTaskQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ProjectTask>>> Handle(ProjectTaskQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<ProjectTask>>();

            result.Value = await _dbContext
                .ProjectTasks
                .Include(pt => pt.Project)
                .Include(pt => pt.User)
                .AsNoTracking()
                .OrderBy(pt => pt.ProjectId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
