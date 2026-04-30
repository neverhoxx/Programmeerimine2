using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectTeams;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectTeamControllerTests : TestBase
    {
        [Fact]
        public async Task Project_team_should_return_paged_result()
        {
            // Arrange
            var url = "/api/ProjectTeam/List/?page=0&pageSize=0";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectTeam>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Get/?id=1";

            await DbContext.AddAsync(CreateTestProject());
            await DbContext.SaveChangesAsync();

            await DbContext.AddAsync(CreateTestUser());
            await DbContext.SaveChangesAsync();

            var team = new ProjectTeam { ProjectId = 1, UserId = 1 };
            await DbContext.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectTeam>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Delete/";

            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var user = CreateTestUser();
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var team = new ProjectTeam { User = user, Project = project };
            await DbContext.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = team.Id })
            };
            using var response = await Client.SendAsync(request);
            var listFromDb = await DbContext.ProjectTeams
                .Where(list => list.Id == team.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_shouldwork_with_missing_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Delete/";

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
        public async Task Save_should_add_new_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Save/";
            var project = new Project { Name = "Test project" };
            var user = new ProjectUser { Name = "Test user", Address="123", Phone="13245", Email="user@example.com" };
            var list = new SaveProjectTeamCommand { ProjectId = 1, UserId = 1 };

            await DbContext.AddAsync(project);
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();            

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectTeamCommand>(url, list);
            var listFromDb = await DbContext.ProjectTeams
                .Where(list => list.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Save/";
            var list = new SaveProjectTeamCommand { Id = 1000, ProjectId = 1, UserId = 1 };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectTeamCommand>(url, list);
            var listFromDb = await DbContext.ProjectTeams
                .Where(list => list.Id == 1000)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_project_team()
        {
            // Arrange
            var url = "/api/ProjectTeam/Save/";
            var list = new SaveProjectTeamCommand {Id = 1000, ProjectId = 1, UserId = 1 };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectTeamCommand>(url, list);
            var listFromDb = await DbContext.ProjectTeams
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