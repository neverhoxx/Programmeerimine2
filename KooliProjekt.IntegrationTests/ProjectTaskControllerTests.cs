using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Projects;
using KooliProjekt.Application.Features.ProjectTasks;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Project = KooliProjekt.Application.Data.Project;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectTaskControllerTests : TestBase
    {
        [Fact]
        public async Task Project_task_should_return_paged_result()
        {
            // Arrange
            var url = "/api/ProjectTask/List/?page=1&pageSize=2";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectTask>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_project_task()
        {
            // Arrange
            var url = "/api/ProjectTask/Get/?id=1";

            await DbContext.AddAsync(CreateTestProject());
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask { Name = "Test task", Description = "Description", Status = "Test status", ProjectId = 1 };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectTask>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_project_task()
        {
            // Arrange
            var url = "/api/ProjectTask/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_project_task()
        {
            // Arrange
            var url = "/api/ProjectTask/Delete/";

            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var user = CreateTestUser();
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask { User = user, Project = project, Name = "Test task", Description = "Description", Status = "Test status" };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = task.Id })
            };
            using var response = await Client.SendAsync(request);
            var listFromDb = await DbContext.ProjectTeams
                .Where(list => list.Id == task.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_shouldwork_with_missing_project_task()
        {
            // Arrange
            var url = "/api/ProjectTask/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_project_task()
        {
            // Arrange
            var url = "/api/Projects/Save/";
            var project = new SaveProjectCommand { Id = 0, Name = "Test ProjectTask" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectCommand>(url, project);
            var listFromDb = await DbContext.Projects
                .Where(list => list.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_project_task()
        {
            // Arrange
            var url = "/api/ProjectTask/Save/";
            var list = new SaveProjectTaskCommand { Id = 10, ProjectId = 0 };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectTaskCommand>(url, list);
            var listFromDb = await DbContext.ProjectTasks
                .Where(list => list.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_project_task()
        {
            // Arrange
            var url = "/api/ProjectTask/Save/";
            var list = new SaveProjectTaskCommand { Id = 0, ProjectId = 0 };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectTaskCommand>(url, list);
            var listFromDb = await DbContext.ProjectTasks
                .Where(list => list.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}