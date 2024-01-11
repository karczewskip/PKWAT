namespace PKWAT.ScoringPoker.Contracts.ScoringTasks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateScoringTaskRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string Name { get; set; }

        [Required]
        public int EstimationMethodId { get; set; }
    }
}
