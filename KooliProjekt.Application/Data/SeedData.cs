using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    /// <summary>
    /// 14.11.2025
    /// Testandmete generaator
    /// 
    /// Testandmed genereeritakse ainult siis kui mõni oluline 
    /// tabel on tühi.
    /// </summary>
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Genereerib andmed
        /// </summary>
        public void Generate()
        {
            if (_dbContext.Projects.Any())
            {
                return;
            }

            for (var i = 0; i < 10; i++)
            {
                var project = new Project
                {
                    Name = "List " + (i + 1),
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddMonths(1),
                    Budget = 1000 + i * 100,
                    PricePerHour = 50 + i * 5
                };
                _dbContext.Projects.Add(project);

                for (var j = 0; j < 5; j++)
                {
                    var task = new ProjectTask
                    {
                        Name = "Item " + (i + 1) + "." + (j + 1),
                        StartDate = DateTime.Now,
                        Status = "Not Started",
                        Project = project
                    };
                    _dbContext.ProjectTasks.Add(task);
                }
            }

            _dbContext.SaveChanges();
        }

        public void GenerateAll()
        {
            if (!_dbContext.Projects.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    var project = new Project
                    {
                        Name = "Project " + (i + 1),
                        StartDate = DateTime.Now.AddDays(-i * 10),
                        DueDate = DateTime.Now.AddMonths(1 + i),
                        Budget = 5000 + i * 1000,
                        PricePerHour = 100 + i * 10
                    };
                    _dbContext.Projects.Add(project);

                    for (var j = 0; j < 5; j++)
                    {
                        var task = new ProjectTask
                        {
                            Name = "Task " + (i + 1) + "." + (j + 1),
                            StartDate = DateTime.Now.AddDays(-j * 5),
                            Status = "Not Started",
                            Project = project,
                            Description = "Description for Task " + (i + 1) + "." + (j + 1)
                        };
                        _dbContext.ProjectTasks.Add(task);

                        for (var k = 0; k < 2; k++)
                        {
                            var workLog = new ProjectWorkLog
                            {
                                Task = task,
                                TimeSpent = 1.5m + k,
                                Date = DateTime.Now.AddDays(-k),
                                UserId = k + 1,
                                Description = "Work log for Task " + (i + 1) + "." + (j + 1)
                            };
                            _dbContext.ProjectWorkLogs.Add(workLog);
                        }
                    }
                }
            }

            if (!_dbContext.ProjectUsers.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    var user = new ProjectUser
                    {
                        Name = "User " + (i + 1),
                        Address = "Address " + (i + 1),
                        Phone = "+123456789" + i,
                        Email = "user" + (i + 1) + "@example.com"
                    };
                    _dbContext.ProjectUsers.Add(user);
                }
            }

            if (!_dbContext.ProjectTeams.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    var projectTeam = new ProjectTeam
                    {
                        ProjectId = (i % 10) + 1,
                        UserId = (i % 10) + 1
                    };
                    _dbContext.ProjectTeams.Add(projectTeam);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
