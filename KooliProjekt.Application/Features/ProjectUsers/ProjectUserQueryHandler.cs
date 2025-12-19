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

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class ProjectUserQueryHandler : IRequestHandler<ProjectUserQuery, OperationResult<PagedResult<ProjectUser>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ProjectUser>>> Handle(ProjectUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<ProjectUser>>();

            result.Value = await _dbContext
                .ProjectUsers
                .Include(pt => pt.Id)
                .Include(pt => pt.Phone)
                .AsNoTracking()
                .OrderBy(pt => pt.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
