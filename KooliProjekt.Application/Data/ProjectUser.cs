using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class ProjectUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^\+\d{6,15}$", ErrorMessage = "Phone must start with + and include country code.")]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public ICollection<ProjectTeam> ProjectTeams { get; set; }
    }
}
