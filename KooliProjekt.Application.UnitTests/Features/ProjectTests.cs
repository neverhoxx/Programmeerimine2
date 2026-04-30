using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Projects;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectTests : TestBase
    {
        private ApplicationDbContext GetFaultyDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            var request = (GetProjectsQuery)null;
            var handler = new GetProjectsQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_invalid(int id)
        {
            var query = new GetProjectsQuery { Id = id };
            var handler = new GetProjectsQueryHandler(GetFaultyDbContext());

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_project()
        {
            var query = new GetProjectsQuery { Id = 1 };
            var handler = new GetProjectsQueryHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ProjectsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ProjectsQuery)null;
            var handler = new ProjectsQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(1, 0)]
        public async Task List_should_return_null_when_page_or_page_size_invalid(int page, int pageSize)
        {
            var query = new ProjectsQuery { Page = page, PageSize = pageSize };
            var handler = new ProjectsQueryHandler(GetFaultyDbContext());

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_projects()
        {
            var query = new ProjectsQuery { Page = 1, PageSize = 5 };
            var handler = new ProjectsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 12))
            {
                var project = new Project { Name = $"Project {i}", StartDate = DateTime.UtcNow };
                await DbContext.Projects.AddAsync(project);
            }
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }
    }
}