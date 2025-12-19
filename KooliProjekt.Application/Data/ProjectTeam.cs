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
        public class ProjectTeam : Entity
        {
            [Required]
            public int ProjectId { get; set; }

            [Required]
            public int UserId { get; set; }

            [ForeignKey(nameof(ProjectId))]
            public Project Project { get; set; }

            [ForeignKey(nameof(UserId))]
            public ProjectUser User { get; set; }

        }
}
