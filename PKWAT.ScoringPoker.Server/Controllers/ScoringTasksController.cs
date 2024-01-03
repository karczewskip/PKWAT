namespace PKWAT.ScoringPoker.Server.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PKWAT.ScoringPoker.Contracts.ScoringTasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ScoringTasksController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetScoringTasks()
        {
            return Ok(new GetScoringTasksResponse
            {
                ScoringTasks = new List<ScoringTaskDto>
                {
                    new ScoringTaskDto
                    {
                        Id = 1,
                        Name = "Planning Poker"
                    }
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateScoringTask(CreateScoringTaskRequest request)
        {
            return Ok(new CreateScoringTaskResponse
            {
                ScoringTask = new ScoringTaskDto
                {
                    Id = 1,
                    Name = request.Name
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScoringTask(int id)
        {
            return Ok();
        }
    }
}
