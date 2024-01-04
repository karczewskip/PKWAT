﻿namespace PKWAT.ScoringPoker.Contracts.EstimationMethods
{
    using System.ComponentModel.DataAnnotations;

    public class CreateEstimationMethodValueRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Value { get; set; }
    }
}