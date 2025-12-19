using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Infrastructure.Paging
{
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Items { get; set; }
    }
}