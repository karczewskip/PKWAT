namespace PKWAT.ScoringPoker.Domain.ScoringTask.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.Entities;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScoringTaskKey : ValueObject
    {
        public int Value { get; protected set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    public class ScoringTask : Entity<ScoringTaskKey>, IAggregateRoot<ScoringTaskKey>
    {
        protected ScoringTask()
        {
        }

        public ScoringTaskName Name { get; protected set; }

        public EstimationMethodKey EstimationMethodKey { get; protected set; }

        public DateTime? EstimationStarted { get; protected set; }

        public DateTime? ScheduledEstimationFinish { get; protected set; }

        public Estimation? FinalEstimation { get; protected set; }

        public ICollection<UserEstimation> TaskEstimations { get; protected set; }

        public ScoringTaskStatus GetStatus(DateTime time)
        {
            if(EstimationStarted == null)
            {
                return ScoringTaskStatus.Created;
            }

            if(FinalEstimation != null)
            {
                return ScoringTaskStatus.Approved;
            }

            if(ScheduledEstimationFinish < time)
            {
                return ScoringTaskStatus.EstimationFinished;
            }

            return ScoringTaskStatus.EstimationStarted;
        }

        public static ScoringTask CreateNew(ScoringTaskName name, EstimationMethodKey estimationMethodKey)
        {
            return new ScoringTask()
            {
                Name = name,
                EstimationMethodKey = estimationMethodKey,
                TaskEstimations = new List<UserEstimation>(),
                EstimationStarted = null,
                FinalEstimation = null
            };
        }

        public void StartEstimation(DateTime time)
        {
            DomainException.ThrowIf(
                GetStatus(time) == ScoringTaskStatus.Approved, 
                "Scoring task is already approved");

            EstimationStarted = time;
            TaskEstimations.Clear();
        }

        public void AppendEstimation(DateTime moment, int userId, Estimation estimation)
        {
            DomainException.ThrowIf(
                GetStatus(moment) != ScoringTaskStatus.EstimationStarted,
                "Scoring task is not in estimation started state");

            DomainException.ThrowIf(
                TaskEstimations.Any(e => e.Id.UserId == userId),
                "User already estimated");

            DomainException.ThrowIf(
                estimation.MethodKey != EstimationMethodKey,
                $"Estimation method for user estimation {estimation.MethodKey} is different from {EstimationMethodKey}");

            TaskEstimations.Add(UserEstimation.CreateNew(UserEstimationKey.Create(userId, Id),estimation, moment));
        }

        public void Approve(Estimation estimation)
        {
            DomainException.ThrowIf(
                FinalEstimation != null,
                "Final estimation is already set.");

            FinalEstimation = estimation;
        }
    }

    public enum ScoringTaskStatus
    {
        Created,
        EstimationStarted,
        EstimationFinished,
        Approved
    }
}
