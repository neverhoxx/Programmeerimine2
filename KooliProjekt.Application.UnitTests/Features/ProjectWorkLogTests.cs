using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectWorkLogs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectWorkLogTests : TestBase
    {
        private ApplicationDbContext GetFaultyDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectWLQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            var request = (GetProjectWLQuery)null;
            var handler = new GetProjectWLQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Get_should_return_object_if_object_exists()
        {
            var query = new GetProjectWLQuery { Id = 1 };
            var workLog = new ProjectWorkLog { Description = "Test WorkLog", Date = DateTime.UtcNow };
            var handler = new GetProjectWLQueryHandler(DbContext);
            await DbContext.ProjectWorkLogs.AddAsync(workLog);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListProjectWLQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ListProjectWLQuery)null;
            var handler = new ListProjectWLQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task List_should_return_page_of_worklogs()
        {
            var query = new ListProjectWLQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectWLQueryHandler(DbContext);

            for (var i = 1; i <= 7; i++)
            {
                var wl = new ProjectWorkLog { Description = $"WL {i}", Date = DateTime.UtcNow, TaskId = i };
                await DbContext.ProjectWorkLogs.AddAsync(wl);
            }
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_no_worklogs_exist()
        {
            var query = new ListProjectWLQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectWLQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteProjectWLCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var request = (DeleteProjectWLCommand)null;
            var handler = new DeleteProjectWLCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            var command = new DeleteProjectWLCommand { Id = id };
            var handler = new DeleteProjectWLCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_worklog()
        {
            var command = new DeleteProjectWLCommand { Id = 1 };
            var handler = new DeleteProjectWLCommandHandler(DbContext);

            var wl = new ProjectWorkLog { Description = "ToDelete", Date = DateTime.UtcNow, TaskId = 1 };
            await DbContext.ProjectWorkLogs.AddAsync(wl);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.ProjectWorkLogs.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_worklog()
        {
            var command = new DeleteProjectWLCommand { Id = 9999 };
            var handler = new DeleteProjectWLCommandHandler(DbContext);

            var wl = new ProjectWorkLog { Description = "Existing", Date = DateTime.UtcNow, TaskId = 1 };
            await DbContext.ProjectWorkLogs.AddAsync(wl);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveProjectWLCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var request = (SaveProjectWLCommand)null;
            var handler = new SaveProjectWLCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            var request = new SaveProjectWLCommand { Id = -10 };
            var handler = new SaveProjectWLCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_worklog()
        {
            var request = new SaveProjectWLCommand
            {
                Id = 0,
                Description = "New WL",
                Date = DateTime.UtcNow,
                TaskId = 1
            };
            var handler = new SaveProjectWLCommandHandler(DbContext);

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.ProjectWorkLogs.SingleOrDefaultAsync(t => t.Id == 1);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_worklog()
        {
            var request = new SaveProjectWLCommand { Id = 1, Description = "Updated", Date = DateTime.UtcNow, TaskId = 2 };
            var handler = new SaveProjectWLCommandHandler(DbContext);

            var wl = new ProjectWorkLog { Description = "Old", Date = DateTime.UtcNow, TaskId = 1 };
            await DbContext.ProjectWorkLogs.AddAsync(wl);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.ProjectWorkLogs.SingleOrDefaultAsync(t => t.Id == request.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Description, saved.Description);
            Assert.Equal(request.TaskId, saved.TaskId);
        }

        [Fact]
        public async Task Save_should_not_update_missing_worklog()
        {
            var request = new SaveProjectWLCommand { Id = 20, Description = "Updated", Date = DateTime.UtcNow, TaskId = 1 };
            var handler = new SaveProjectWLCommandHandler(DbContext);

            var wl = new ProjectWorkLog { Description = "Old", Date = DateTime.UtcNow, TaskId = 1 };
            await DbContext.ProjectWorkLogs.AddAsync(wl);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}
