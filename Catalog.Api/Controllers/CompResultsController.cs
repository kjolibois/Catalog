using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitionResultsController : ControllerBase
    {
        private readonly ICompResultsRepository repository;
        private readonly ILogger<CompetitionResultsController> logger;
        public CompetitionResultsController(ICompResultsRepository _repository,ILogger<CompetitionResultsController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
             // get
        [HttpGet]
        public async Task<IEnumerable<CompResultDto>> GetCompetitionResultsAsync()
        {
            var results  = (await repository.GetCompResultsAsync()).Select(baker => baker.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {results.Count()} results");
            return results;
        }
        //get results/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetCompetitionResultAsync))]

        public async Task<ActionResult<CompResultDto>> GetCompetitionResultAsync(Guid id)
        {
            var result = await repository.GetCompResultAsync(id);
            if (result is null)
            {
                return NotFound();
            }
            return result.AsDto(); 
        }
        // POST /results
        [HttpPost]
        public async Task<ActionResult<CompResultDto>> CreateCompetitionResultAsync (CreateCompResultDto resultDto)
        {
                CompetitionResult result = new ()
                {
                    Id = Guid.NewGuid(),

                    CreatedDate = DateTimeOffset.UtcNow
                };
                await repository.CreateCompResultAsync(result);
                return CreatedAtAction(nameof(GetCompetitionResultAsync),new {id = result.Id}, result.AsDto());

        } 
        //Task<ActionResult<List<CompetitionResultDto>>> 
       // POST /results
       /*
        [Route("result")]
        [HttpPost]

        public async Task<int> CreateCompetitionResultsByCompetitionResultAsync ([FromBody] CreateByCompResult resultDtoList)
        {
            Console.WriteLine(resultDtoList.results[0].CompetitionResultName);
            //CreateByCompetitionResult myDeserializedClass = JsonSerializer.Deserialize<CreateByCompetitionResult>(resultDtoList);
            List<CompetitionResult> resultslist= new List<CompetitionResult>();
                 foreach (var item in resultDtoList.results)
                 {
                CompetitionResult result = new ()
                {
                    Id = Guid.NewGuid(),
 

                    CreatedDate = DateTimeOffset.UtcNow
                };
                resultslist.Add(result);
                 }
               await repository.CreateMultipleCompResultsAsync(resultslist);
               // return CreatedAtAction(nameof(GetCompetitionResultAsync),new {id = result.Id}, result.AsDto());
              return 4;
        } */
      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateCompetitionResultAsync(Guid id, UpdateCompResultDto resultDto)
      {
          var existingCompetitionResult = await repository.GetCompResultAsync(id);
          if (existingCompetitionResult is null)
          {
              return NotFound();
          }
          CompetitionResult updateCompetitionResult = existingCompetitionResult with 
          {
  
          };
          await repository.UpdateCompResultAsync(updateCompetitionResult);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteCompetitionResultAsync(Guid id)
      {
          var existingCompetitionResult = await repository.GetCompResultAsync(id);
          if (existingCompetitionResult is null)
          {
              return NotFound();
          }

          await repository.DeleteCompResultAsync(id);
          return NoContent();
      }

    }
    
}