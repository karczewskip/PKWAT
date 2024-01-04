namespace PKWAT.ScoringPoker.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PKWAT.ScoringPoker.Server.Data;
    using PKWAT.ScoringPoker.Contracts.EstimationMethods;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.Entities;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;

    [Route("api/[controller]")]
    [ApiController]
    public class EstimationMethodsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public EstimationMethodsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstimationMethods(CancellationToken cancellationToken)
        {
            var estimationMethods = await _dbContext
                .EstimationMethods
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            return Ok(new GetEstimationMethodsResponse
            {
                EstimationMethods = estimationMethods.Select(x => new EstimationMethodDto
                {
                    Id = x.Id,
                    Name = x.Name.Value
                })
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstimationMethodWithValues(int id, CancellationToken cancellationToken)
        {
            var estimationMethodWithValues = await _dbContext
                .EstimationMethods
                .Include(x => x.PossibleValues)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if(estimationMethodWithValues is null)
            {
                return BadRequest("Estimation method with given id does not exist.");
            }

            return Ok(new GetEstimationMethodWithValuesResponse
            {
                EstimationMethod = new EstimationMethodDto 
                { 
                    Id = estimationMethodWithValues.Id, 
                    Name = estimationMethodWithValues.Name.Value 
                },
                EstimationMethodValues = estimationMethodWithValues
                    .PossibleValues
                    .Select(x => new EstimationMethodValueDto 
                    { 
                        Id = x.Id, 
                        Value = x.EstimationMethodValue.Value
                    })
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstimationMethod(CreateEstimationMethodRequest request, CancellationToken cancellationToken)
        {
            var newEstimationMethod = EstimationMethod.CreateNew(EstimationMethodName.Create(request.Name));

            var entry = await _dbContext
                .EstimationMethods
                .AddAsync(newEstimationMethod, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(new CreateEstimationMethodResponse
            {
                EstimationMethod = new EstimationMethodDto
                {
                    Id = entry.Entity.Id,
                    Name = entry.Entity.Name.Value
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstimationMethod(int id, CancellationToken cancellationToken)
        {
            await _dbContext
                .EstimationMethods
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return Ok();
        }

        [HttpGet("{estimationMethodId}/possible-values")]
        public async Task<IActionResult> GetEstimationMethodValues(int estimationMethodId, CancellationToken cancellationToken)
        {
            var estimationMethodPossibleValues = await _dbContext
                .EstimationMethodPossibleValues
                .AsNoTracking()
                .Where(x => x.EstimationMethodId == estimationMethodId)
                .ToArrayAsync(cancellationToken);

            return Ok(new GetEstimationMethodValuesResponse
            {
                EstimationMethodId = estimationMethodId,
                EstimationMethodValues = estimationMethodPossibleValues.Select(x => new EstimationMethodValueDto 
                { 
                    Id = x.Id,
                    Value = x.EstimationMethodValue.Value
                })
            });
        }

        [HttpPost("{estimationMethodId}/possible-values")]
        public async Task<IActionResult> CreateEstimationMethodValue(int estimationMethodId, CreateEstimationMethodValueRequest request, CancellationToken cancellationToken)
        {
            var estimationMethod = await _dbContext
                .EstimationMethods
                .Include(x => x.PossibleValues)
                .FirstOrDefaultAsync(x => x.Id == estimationMethodId, cancellationToken);

            if(estimationMethod is null)
            {
                return BadRequest("Estimation method with given id does not exist.");
            }

            var newPossibleValue = estimationMethod.AddPossibleValue(EstimationMethodValue.Create(request.Value));

            var entry = _dbContext.Entry(newPossibleValue);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(new CreateEstimationMethodValueResponse
            {
                Value = new EstimationMethodValueDto 
                {
                    Id = entry.Entity.Id,
                    Value = entry.Entity.EstimationMethodValue.Value
                }
            });
        }

        [HttpDelete("{estimationMethodId}/possible-values/{valueId}")]
        public async Task<IActionResult> DeleteEstimationMethodsValue(int estimationMethodId, int valueId, CancellationToken cancellationToken)
        {
            await _dbContext
                .EstimationMethodPossibleValues
                .Where(x => x.EstimationMethodId == estimationMethodId && x.Id == valueId)
                .ExecuteDeleteAsync(cancellationToken);

            return Ok();
        }
    }
}
