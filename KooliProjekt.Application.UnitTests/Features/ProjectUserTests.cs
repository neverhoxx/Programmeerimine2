using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectUsers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectUserTests : TestBase
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
                new GetProjectUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            var request = (GetProjectUserQuery)null;
            var handler = new GetProjectUserQueryHandler(DbContext);

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
            var query = new GetProjectUserQuery { Id = id };
            var handler = new GetProjectUserQueryHandler(GetFaultyDbContext());

            var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address = "Address", Phone = "00000" };
            await DbContext.ProjectUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            var query = new GetProjectUserQuery { Id = 1 };
            var handler = new GetProjectUserQueryHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address = "Address", Phone = "00000" };
            await DbContext.Projects.AddAsync(project);
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.ProjectUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_user_does_not_exist(int id)
        {
            var query = new GetProjectUserQuery { Id = id };
            var handler = new GetProjectUserQueryHandler(DbContext);

            var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address = "Address", Phone = "00000" };
            await DbContext.ProjectUsers.AddAsync(user);
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
                new ListProjectUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ListProjectUserQuery)null;
            var handler = new ListProjectUserQueryHandler(DbContext);

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

            var query = new ListProjectUserQuery { Page = page, PageSize = pageSize };
            var handler = new ListProjectUserQueryHandler(GetFaultyDbContext());

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_users()
        {
            var query = new ListProjectUserQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectUserQueryHandler(DbContext);

            for (var i = 1; i <= 8; i++)
            {
                var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address = "Address", Phone = "00000" };
                await DbContext.ProjectUsers.AddAsync(user);
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
        public async Task List_should_return_empty_result_if_no_users_exist()
        {
            var query = new ListProjectUserQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectUserQueryHandler(DbContext);

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
                new DeleteProjectUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var request = (DeleteProjectUserCommand)null;
            var handler = new DeleteProjectUserCommandHandler(DbContext);

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
            var command = new DeleteProjectUserCommand { Id = id };
            var handler = new DeleteProjectUserCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_user()
        {
            var command = new DeleteProjectUserCommand { Id = 1 };
            var handler = new DeleteProjectUserCommandHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };            
            var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address="Address", Phone="00000" };
            await DbContext.Projects.AddAsync(project);
            await DbContext.ProjectTeams.AddAsync(team);
            await DbContext.ProjectUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.ProjectUsers.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_user()
        {
            var command = new DeleteProjectUserCommand { Id = 9999 };
            var handler = new DeleteProjectUserCommandHandler(DbContext);

            var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address = "Address", Phone = "00000" };
            await DbContext.ProjectUsers.AddAsync(user);
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
                new SaveProjectUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var request = (SaveProjectUserCommand)null;
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            var request = new SaveProjectUserCommand { Id = -10 };
            var handler = new SaveProjectUserCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_user()
        {
            var request = new SaveProjectUserCommand
            {
                Id = 0,
                Email = "new@test.local",
                Name = "New user",
                Address = "Address",
                Phone = "00000"
            };
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.ProjectUsers.SingleOrDefaultAsync(t => t.Id == 1);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_user()
        {
            var request = new SaveProjectUserCommand { Id = 1, Name = "Updated", Email = "updated.local", Address="Address1", Phone="Phone1" };
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var user = new ProjectUser { Name = "Old", Email = "old@test.local", Address = "Address", Phone = "00000" };
            await DbContext.ProjectUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.ProjectUsers.SingleOrDefaultAsync(t => t.Id == request.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Name, saved.Name);
            Assert.Equal(request.Email, saved.Email);
        }

        [Fact]
        public async Task Save_should_not_update_missing_user()
        {
            var request = new SaveProjectUserCommand { Id = 20, Name = "Updated", Email = "u@updated.local" };
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var user = new ProjectUser { Name = "ToDelete", Email = "d@test.local", Address = "Address", Phone = "00000" };
            await DbContext.ProjectUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}