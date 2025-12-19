using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.Projects
{
    public class GetProjectsQuery : IRequest<OperationResult<object>>, IBaseRequest
    {
        public int Id { get; set; }
    }
}
