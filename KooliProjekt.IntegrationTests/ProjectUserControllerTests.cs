using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectUsers;
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
    public class ProjectUserControllerTests : TestBase
    {
        [Fact]
        public async Task Project_user_should_return_paged_result()
        {
            // Arrange
            var url = "/api/ProjectUser/List/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectUser>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Get/?id=1";

            var user = CreateTestUser();
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectUser>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Delete/";

            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var user = CreateTestUser();
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = user.Id })
            };
            using var response = await Client.SendAsync(request);
            var listFromDb = await DbContext.ProjectWorkLogs
                .Where(list => list.Id == user.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_shouldwork_with_missing_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Delete/";

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
        public async Task Save_should_add_new_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Save/";
            var list = new SaveProjectUserCommand { Name = "Test List", Address = "sõprusepst", Phone = "53545159", Email = "yehorkiiakh@example.com" };


            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectUserCommand>(url, list);
            var listFromDb = await DbContext.ProjectUsers
                .Where(list => list.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Save/";
            var list = new SaveProjectUserCommand { Id = 10, Name = "Test List" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectUserCommand>(url, list);
            var listFromDb = await DbContext.ProjectUsers
                .Where(list => list.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_project_user()
        {
            // Arrange
            var url = "/api/ProjectUser/Save/";
            var list = new SaveProjectUserCommand { Id = 0, Name = "" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveProjectUserCommand>(url, list);
            var listFromDb = await DbContext.ProjectUsers
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