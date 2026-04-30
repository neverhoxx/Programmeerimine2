using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Project>>> List(int page, int pageSize);
        Task<OperationResult> Save(Project project);
        Task<OperationResult> Delete(int id);
    }
}
