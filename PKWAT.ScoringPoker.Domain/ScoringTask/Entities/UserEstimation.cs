namespace PKWAT.ScoringPoker.Domain.ScoringTask.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using System;

    public class UserEstimation : Entity<int>
    {
        public int UserId { get; protected set; }
        public int ScoringTaskId { get; protected set; }
        public int EstimationMethodId { get; protected set; }
        public EstimationMethodValue Value { get; protected set; }
        public DateTime Moment { get; protected set; }

        protected UserEstimation()
        {
        }

        public static UserEstimation CreateNew(int userId, int scoringTaskId, int estimationMethodId, EstimationMethodValue estimationMethodValue, DateTime moment)
        {
            return new UserEstimation()
            {
                UserId = userId,
                ScoringTaskId = scoringTaskId,
                EstimationMethodId = estimationMethodId,
                Value = estimationMethodValue,
                Moment = moment
            };
        }
    }
}
