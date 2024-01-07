namespace PKWAT.ScoringPoker.Domain.ScoringTask.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.Entities;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScoringTask : Entity<int>, IAggregateRoot<int>
    {
        protected ScoringTask()
        {
        }

        public ScoringTaskName Name { get; protected set; }
        
        public ScoringTaskStatusId Status { get; protected set; }

        public int EstimationMethodId { get; protected set; }

        public EstimationMethod EstimationMethod { get; protected set; }

        public int OwnerId { get; protected set; }

        public DateTime? EstimationStarted { get; protected set; }

        public DateTime? ScheduledEstimationFinish { get; protected set; }

        public ICollection<UserEstimation> UserEstimations { get; protected set; }

        public EstimationMethodName? FinalEstimationMethodName { get; protected set; }

        public EstimationMethodValue? FinalEstimationValue { get; protected set; }

        public static ScoringTask CreateNew(ScoringTaskName name, int estimationMethodId, int ownerId)
        {
            return new ScoringTask()
            {
                Name = name,
                Status = ScoringTaskStatusId.Created,
                EstimationMethodId = estimationMethodId,
                OwnerId = ownerId,
                UserEstimations = new List<UserEstimation>(),
            };
        }

        public void StartEstimation(int userId, DateTime time, DateTime finishTime)
        {
            DomainException.ThrowIf(
                Status is ScoringTaskStatusId.EstimationStarted or ScoringTaskStatusId.Approved,
                $"Scoring task cannot be started in state {Status}");

            DomainException.ThrowIf(
                OwnerId != userId,
                "Only owner can start estimation");

            Status = ScoringTaskStatusId.EstimationStarted;
            EstimationStarted = time;
            ScheduledEstimationFinish = finishTime;
            UserEstimations.Clear();
        }

        public void FinishEstimation()
        {
            Status = ScoringTaskStatusId.EstimationFinished;
        }

        public bool CanBeStarted()
        {
            return Status is ScoringTaskStatusId.Created or ScoringTaskStatusId.EstimationFinished;
        }

        public bool CanAppendUserEstimation()
        {
            return Status is ScoringTaskStatusId.EstimationStarted;
        }

        public bool CanShowUserEstimationValues()
        {
            return Status is ScoringTaskStatusId.EstimationFinished;
        }

        public void AppendEstimation(DateTime moment, int userId, int estimationMethodId, EstimationMethodValue estimationValue)
        {
            DomainException.ThrowIf(
                Status is not ScoringTaskStatusId.EstimationStarted,
                "Scoring task is not in estimation started state");

            DomainException.ThrowIf(
                estimationMethodId != EstimationMethodId,
                $"Estimation method for user estimation {estimationMethodId} is different from {EstimationMethodId}");

            DomainException.ThrowIf(
                moment > ScheduledEstimationFinish,
                "Estimation is too late");

            var previousEstimation = UserEstimations.FirstOrDefault(x => x.UserId == userId);
            if(previousEstimation is not null)
            {
                UserEstimations.Remove(previousEstimation);
            }

            UserEstimations.Add(UserEstimation.CreateNew(userId, Id, EstimationMethod.Name, estimationValue, moment));
        }

        public void Approve(int userId, int estimationMethodId, EstimationMethodValue estimation)
        {
            DomainException.ThrowIf(
                OwnerId != userId,
                "Only owner can set final estimation");

            DomainException.ThrowIf(
                Status is ScoringTaskStatusId.Approved,
                "Final estimation is already set.");

            DomainException.ThrowIf(
                estimationMethodId != EstimationMethodId,
                $"Estimation method for final estimation {estimationMethodId} is different from {EstimationMethodId}");

            Status = ScoringTaskStatusId.Approved;
            FinalEstimationMethodName = EstimationMethod.Name;
            FinalEstimationValue = estimation;
        }

        public bool CanBeApprovedByOwner()
        {
            return Status is ScoringTaskStatusId.EstimationFinished;
        }

        public bool CanShowFinalEstimationValue()
        {
            return Status is ScoringTaskStatusId.Approved;
        }
    }

    public enum ScoringTaskStatusId
    {
        Created,
        EstimationStarted,
        EstimationFinished,
        Approved
    }

    public static class ScoringTaskStatusIdExtensions
    {
        public static ScoringTaskStatus ToScoringTaskStatus(this ScoringTaskStatusId id)
        {
            return ScoringTaskStatus.Create(id);
        }

        public static string ToFriendlyString(this ScoringTaskStatusId id)
        {
            switch (id)
            {
                case ScoringTaskStatusId.Created:
                    return "Created";
                case ScoringTaskStatusId.EstimationStarted:
                    return "Estimation started";
                case ScoringTaskStatusId.EstimationFinished:
                    return "Estimation finished";
                case ScoringTaskStatusId.Approved:
                    return "Approved";
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }
    }

    public class ScoringTaskStatus : Entity<ScoringTaskStatusId>
    {
        public string Name { get; protected set; }

        protected ScoringTaskStatus()
        {}

        public static ScoringTaskStatus Create(ScoringTaskStatusId id)
        {
            return new ScoringTaskStatus()
            {
                Id = id,
                Name = id.ToFriendlyString()
            };
        }
    }
}
