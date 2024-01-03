namespace PKWAT.ScoringPoker.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.ScoringTasks;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;
    using PKWAT.ScoringPoker.Server.Data;

    [Route("api/[controller]")]
    [ApiController]
    public class ScoringTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ScoringTasksController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetScoringTasks(CancellationToken cancellationToken)
        {
            var scoringTasks = await _dbContext.ScoringTasks.ToListAsync(cancellationToken);

            return Ok(new GetScoringTasksResponse
            {
                ScoringTasks = scoringTasks.Select(x => new ScoringTaskDto
                {
                    Id = x.Id,
                    Name = x.Name.Name
                })
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateScoringTask(CreateScoringTaskRequest request, CancellationToken cancellationToken)
        {
            var newScoringTask = ScoringTask.CreateNew(ScoringTaskName.Create(request.Name));

            var entry = await _dbContext.ScoringTasks.AddAsync(newScoringTask, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(new CreateScoringTaskResponse
            {
                ScoringTask = new ScoringTaskDto
                {
                    Id = entry.Entity.Id,
                    Name = entry.Entity.Name.Name
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScoringTask(int id, CancellationToken cancellationToken)
        {
            await _dbContext.ScoringTasks.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);

            return Ok();
        }
    }
}
