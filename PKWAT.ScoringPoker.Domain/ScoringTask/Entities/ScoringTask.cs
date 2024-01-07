﻿namespace PKWAT.ScoringPoker.Domain.ScoringTask.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.Entities;
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

        //public ICollection<UserEstimation> TaskEstimations { get; protected set; }

        //public Estimation? FinalEstimation { get; protected set; }

        public static ScoringTask CreateNew(ScoringTaskName name, int estimationMethodId, int ownerId)
        {
            return new ScoringTask()
            {
                Name = name,
                Status = ScoringTaskStatusId.Created,
                EstimationMethodId = estimationMethodId,
                OwnerId = ownerId,
                //TaskEstimations = new List<UserEstimation>(),
                //EstimationStarted = null,
                //FinalEstimation = null
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
            //TaskEstimations.Clear();
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
            return Status == ScoringTaskStatusId.EstimationStarted;
        }

        //public void AppendEstimation(DateTime moment, int userId, Estimation estimation)
        //{
        //    DomainException.ThrowIf(
        //        GetStatus(moment) != ScoringTaskStatus.EstimationStarted,
        //        "Scoring task is not in estimation started state");

        //    DomainException.ThrowIf(
        //        TaskEstimations.Any(e => e.Id.UserId == userId),
        //        "User already estimated");

        //    DomainException.ThrowIf(
        //        estimation.MethodKey != EstimationMethodKey,
        //        $"Estimation method for user estimation {estimation.MethodKey} is different from {EstimationMethodKey}");

        //    TaskEstimations.Add(UserEstimation.CreateNew(UserEstimationKey.Create(userId, Id),estimation, moment));
        //}

        //public void Approve(Estimation estimation)
        //{
        //    DomainException.ThrowIf(
        //        FinalEstimation != null,
        //        "Final estimation is already set.");

        //    FinalEstimation = estimation;
        //}
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
