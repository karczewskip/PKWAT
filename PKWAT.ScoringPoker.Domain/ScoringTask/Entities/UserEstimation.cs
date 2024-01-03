namespace PKWAT.ScoringPoker.Domain.ScoringTask.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;
    using System;

    public class UserEstimation : Entity<UserEstimationKey>
    {
        public Estimation Estimation { get; protected set; }
        public DateTime Moment { get; protected set; }

        protected UserEstimation()
        {
        }

        public static UserEstimation CreateNew(UserEstimationKey key, Estimation estimation, DateTime moment)
        {
            return new UserEstimation()
            {
                Id = key,
                Estimation = estimation,
                Moment = moment
            };
        }
    }

    public class UserEstimationKey : ValueObject
    {
        public int UserId { get; protected set; }
        public int ScoringTaskId { get; protected set; }

        private UserEstimationKey() { }

        public static UserEstimationKey Create(int userId, int scoringTaskId)
        {
            return new UserEstimationKey()
            {
                UserId = userId,
                ScoringTaskId = scoringTaskId
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return ScoringTaskId;
        }
    }
}
