using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectTasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectTaskTests : TestBase
    {
        private ApplicationDbContext GetFaultyDbContext()
        {
            // Return a separate in-memory context instance for tests where
            // handler should not rely on the shared DbContext state.
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
                new GetProjectTaskQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetProjectTaskQuery)null;
            var handler = new GetProjectTaskQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_null_or_negative(int id)
        {
            // Arrange
            var query = new GetProjectTaskQuery { Id = id };
            var handler = new GetProjectTaskQueryHandler(GetFaultyDbContext());

            var task = new ProjectTask { ProjectId = 1, Name = "Test Task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_task()
        {
            // Arrange
            var query = new GetProjectTaskQuery { Id = 1 };
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            var task = new ProjectTask { ProjectId = 1, Name = "Test task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.Projects.AddAsync(project);
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_task_does_not_exist(int id)
        {
            // Arrange
            var query = new GetProjectTaskQuery { Id = id };
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var task = new ProjectTask { ProjectId = 1, Name = "Test task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListProjectTaskQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListProjectTaskQuery)null;
            var handler = new ListProjectTaskQueryHandler(DbContext);

            // Act && Assert
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
            // Arrange
            var query = new ListProjectTaskQuery { Page = page, PageSize = pageSize };
            var handler = new ListProjectTaskQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_tasks()
        {
            // Arrange
            var query = new ListProjectTaskQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectTaskQueryHandler(DbContext);
            var project = new Project { Name = "Test project", StartDate = DateTime.UtcNow };
            await DbContext.Projects.AddAsync(project);

            foreach (var i in Enumerable.Range(1, 15))
            {
                var task = new ProjectTask { ProjectId = i % 3 + 1, Name = $"Test task {i}", StartDate = DateTime.UtcNow, Status = "New" };
                await DbContext.ProjectTasks.AddAsync(task);
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_tasks_doesnt_exist()
        {
            // Arrange
            var query = new ListProjectTaskQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectTaskQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
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
                new DeleteProjectTaskCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteProjectTaskCommand)null;
            var handler = new DeleteProjectTaskCommandHandler(DbContext);

            // Act && Assert
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
            // Arrange
            var query = new DeleteProjectTaskCommand { Id = id };
            var handler = new DeleteProjectTaskCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_task()
        {
            // Arrange
            var query = new DeleteProjectTaskCommand { Id = 1 };
            var handler = new DeleteProjectTaskCommandHandler(DbContext);

            var task = new ProjectTask { ProjectId = 1, Name = "Test task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.ProjectTasks.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_task()
        {
            // Arrange
            var query = new DeleteProjectTaskCommand { Id = 1034 };
            var handler = new DeleteProjectTaskCommandHandler(DbContext);

            var task = new ProjectTask { ProjectId = 1, Name = "Test task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveProjectTaskCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveProjectTaskCommand)null;
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            // Arrange
            var request = new SaveProjectTaskCommand { Id = -10 };
            var handler = new SaveProjectTaskCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_task()
        {
            // Arrange
            var request = new SaveProjectTaskCommand
            {
                Id = 0,
                ProjectId = 1,
                UserId = null,
                Name = "New task",
                StartDate = DateTime.UtcNow,
                Status = "New"
            };
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedTask = await DbContext.ProjectTasks.SingleOrDefaultAsync(t => t.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTask);
            Assert.Equal(1, savedTask.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_task()
        {
            // Arrange
            var request = new SaveProjectTaskCommand { Id = 1, ProjectId = 1, UserId = null, Name = "Updated task", StartDate = DateTime.UtcNow, Status = "Updated" };
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            var task = new ProjectTask { Id = 0, ProjectId = 1, Name = "New task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedTask = await DbContext.ProjectTasks.SingleOrDefaultAsync(t => t.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTask);
            Assert.Equal(request.Name, savedTask.Name);
        }

        [Fact]
        public async Task Save_should_not_update_missing_task()
        {
            // Arrange
            var request = new SaveProjectTaskCommand { Id = 20, ProjectId = 1, Name = "Updated task", StartDate = DateTime.UtcNow, Status = "Updated" };
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            var task = new ProjectTask { Id = 0, ProjectId = 1, Name = "New task", StartDate = DateTime.UtcNow, Status = "New" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123456789000")]
        public void SaveValidator_should_return_false_when_name_is_invalid(string name)
        {
            // Arrange
            var command = new SaveProjectTaskCommand { Id = 0, Name = name, ProjectId = 1, StartDate = DateTime.UtcNow, Status = "New" };
            var validator = new SaveProjectTaskCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveProjectTaskCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_name_is_valid()
        {
            // Arrange
            var command = new SaveProjectTaskCommand { Id = 0, Name = "New task", ProjectId = 1, StartDate = DateTime.UtcNow, Status = "New" };
            var validator = new SaveProjectTaskCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}