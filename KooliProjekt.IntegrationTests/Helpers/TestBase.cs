using System;
using System.Net.Http;
using KooliProjekt.Application.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;

        public WebApplicationFactory<FakeStartup> Factory { get; }
        public HttpClient Client { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
            Client = Factory.CreateClient();
        }

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                _dbContext = Factory.Services.GetService<ApplicationDbContext>();
                return _dbContext;
            }
        }

        protected ProjectTask CreateTestTask()
        {
            return new ProjectTask
            {
                Name = "test task",
                StartDate = DateTime.Now,
                Description = "test description",
                ProjectId = 1,
                UserId = 1,
                Status = "Not Started",                
            };
        }

        protected ProjectUser CreateTestUser()
        {
            return new ProjectUser 
            { 
                Name = "Username",
                Address = "Testi 123",
                Phone = "3725543256",
                Email = "test@example.com"
            };            
        }

        protected Project CreateTestProject()
        {
            return new Project
            {
                Name = "Test Project"
            };
        }

        protected ProjectWorkLog CreateTestProjectWorkLog()
        {
            return new ProjectWorkLog 
            { 
                UserId = 1, 
                TaskId = 1, 
                Date = DateTime.Now, 
                TimeSpent = 100, 
                Description = "Description" 
            };
        }

        public void Dispose()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
        }

        // Add your other helper methods here
    }
}