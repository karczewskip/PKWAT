﻿namespace PKWAT.ScoringPoker.Server.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.ScoringTasks;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;
    using PKWAT.ScoringPoker.Server.Data;
    using PKWAT.ScoringPoker.Server.Extensions;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScoringTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScoringTasksController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetScoringTasks(CancellationToken cancellationToken)
        {
            var scoringTasks = await _dbContext
                .ScoringTasks
                .Include(x => x.EstimationMethod)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Ok(new GetScoringTasksResponse
            {
                ScoringTasks = scoringTasks.Select(x => x.ToDto())
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateScoringTask(CreateScoringTaskRequest request, CancellationToken cancellationToken)
        {
            var estimationMethod = await _dbContext
                .EstimationMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.EstimationMethodId, cancellationToken);

            if (estimationMethod is null)
            {
                return BadRequest($"No estimation method with id: {request.EstimationMethodId}");
            }

            var owner = await _userManager.GetUserAsync(User);

            if (owner == null)
            {
                return Unauthorized($"Cannot get user {User?.Identity?.Name}");
            }

            var newScoringTask = ScoringTask.CreateNew(ScoringTaskName.Create(request.Name), request.EstimationMethodId, owner.Id);

            var entry = await _dbContext
                .ScoringTasks
                .AddAsync(newScoringTask, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(new CreateScoringTaskResponse
            {
                ScoringTask = entry.Entity.ToDto()
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScoringTask(int id, CancellationToken cancellationToken)
        {
            await _dbContext
                .ScoringTasks
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return Ok();
        }
    }
}
