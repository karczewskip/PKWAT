namespace PKWAT.ScoringPoker.Contracts.EstimationMethods
{
    using System.ComponentModel.DataAnnotations;

    public class CreateEstimationMethodRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string Name { get; set; }
    }
}
