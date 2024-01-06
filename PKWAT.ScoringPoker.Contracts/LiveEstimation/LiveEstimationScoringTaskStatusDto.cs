namespace PKWAT.ScoringPoker.Contracts.LiveEstimation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LiveEstimationScoringTaskStatusDto
    {
        public required string ScoringTaskName { get; set; }
        public required string ScoringTaskStatus { get; set; }
        public required string[] ScoringTaskObservers { get; set; }
        public required string ScoringTaskOwner { get; set; }
    }
}
