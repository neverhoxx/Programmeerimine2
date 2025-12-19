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

namespace KooliProjekt.Application.Features.ProjectWorkLogs
{
    public class ProjectWLQueryHandler : IRequestHandler<ProjectWLQuery, OperationResult<PagedResult<ProjectWorkLog>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectWLQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ProjectWorkLog>>> Handle(ProjectWLQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<ProjectWorkLog>>();

            result.Value = await _dbContext
                .ProjectWorkLogs
                .Include(pt => pt.Date)
                .Include(pt => pt.User)
                .AsNoTracking()
                .OrderBy(pt => pt.TaskId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
