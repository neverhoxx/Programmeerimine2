using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectTeams;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectTeamTests : TestBase
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
                new GetProjectTeamQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            var request = (GetProjectTeamQuery)null;
            var handler = new GetProjectTeamQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_zero_or_negative(int id)
        {
            var query = new GetProjectTeamQuery { Id = id };
            var handler = new GetProjectTeamQueryHandler(GetFaultyDbContext());

            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_team_entry()
        {
            var query = new GetProjectTeamQuery { Id = 1 };
            var handler = new GetProjectTeamQueryHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.AddAsync(project);
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_team_does_not_exist(int id)
        {
            var query = new GetProjectTeamQuery { Id = id };
            var handler = new GetProjectTeamQueryHandler(DbContext);

            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListProjectTeamQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ListProjectTeamQuery)null;
            var handler = new ListProjectTeamQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(4, -10)]
        [InlineData(5, -5)]
        [InlineData(0, 0)]
        [InlineData(-5, -10)]
        public async Task List_should_return_null_when_page_or_page_size_is_zero_or_negative(int page, int pageSize)
        {
            var query = new ListProjectTeamQuery { Page = page, PageSize = pageSize };
            var handler = new ListProjectTeamQueryHandler(GetFaultyDbContext());

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_team_entries()
        {
            var query = new ListProjectTeamQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectTeamQueryHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            await DbContext.AddAsync(project);

            for (var i = 1; i <= 10; i++)
            {
                var team = new ProjectTeam { ProjectId = 1, UserId = i };
                await DbContext.ProjectTeams.AddAsync(team);
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
        public async Task List_should_return_empty_result_if_no_team_entries_exist()
        {
            var query = new ListProjectTeamQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectTeamQueryHandler(DbContext);

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
                new DeleteProjectTeamCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var request = (DeleteProjectTeamCommand)null;
            var handler = new DeleteProjectTeamCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            var command = new DeleteProjectTeamCommand { Id = id };
            var handler = new DeleteProjectTeamCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_team()
        {
            var command = new DeleteProjectTeamCommand { Id = 1 };
            var handler = new DeleteProjectTeamCommandHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.AddAsync(project);
            await DbContext.AddAsync(team);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.ProjectTeams.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_team()
        {
            var command = new DeleteProjectTeamCommand { Id = 9999 };
            var handler = new DeleteProjectTeamCommandHandler(DbContext);

            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.ProjectTeams.AddAsync(team);
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
                new SaveProjectTeamCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var request = (SaveProjectTeamCommand)null;
            var handler = new SaveProjectTeamCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            var request = new SaveProjectTeamCommand { Id = -10 };
            var handler = new SaveProjectTeamCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_team()
        {
            var request = new SaveProjectTeamCommand
            {
                Id = 0,
                ProjectId = 1,
                UserId = 1
            };
            var handler = new SaveProjectTeamCommandHandler(DbContext);

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.ProjectTeams.SingleOrDefaultAsync(t => t.Id == 1);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_team()
        {
            var request = new SaveProjectTeamCommand { Id = 1, ProjectId = 2, UserId = 42 };
            var handler = new SaveProjectTeamCommandHandler(DbContext);

            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.ProjectTeams.SingleOrDefaultAsync(t => t.Id == request.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.ProjectId, saved.ProjectId);
            Assert.Equal(request.UserId, saved.UserId);
        }

        [Fact]
        public async Task Save_should_not_update_missing_team()
        {
            var request = new SaveProjectTeamCommand { Id = 20, ProjectId = 1, UserId = 1 };
            var handler = new SaveProjectTeamCommandHandler(DbContext);

            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}