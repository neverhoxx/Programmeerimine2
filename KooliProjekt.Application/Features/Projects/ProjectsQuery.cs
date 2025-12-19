using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Projects
{
    public class ProjectsQuery : IRequest<OperationResult<PagedResult<Project>>>
    {
        public int Page { get; set; } = 1; 
        public int PageSize { get; set; } = 10; 
    }
}