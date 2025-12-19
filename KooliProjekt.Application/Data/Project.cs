using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Project : Entity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Budget cannot be negative.")]
        public decimal Budget { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price per hour must be >= 0.")]
        public decimal PricePerHour { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
        public ICollection<ProjectTeam> ProjectTeams { get; set; }
    }
}
