using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public decimal? Price { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ProjectUser User { get; set; }

        [MaxLength(500)]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters.")]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        public ICollection<ProjectWorkLog> WorkLogs { get; set; }
    }
}
