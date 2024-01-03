namespace PKWAT.ScoringPoker.Contracts.ScoringTasks
{
    using System;

    public class ScoringTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EstimationMethod { get; set; }
        public DateTime? EstimationStarted { get; set; }
        public DateTime? ScheduledEstimationFinish { get; set; }
        public string FinalEstimation { get; set; }
        public string Status { get; set; }
    }
}
