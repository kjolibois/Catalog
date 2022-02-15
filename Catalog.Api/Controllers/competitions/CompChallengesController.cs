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
    public class CompetitionChallengesController : ControllerBase
    {
        private readonly ICompChallengesRepository repository;
        private readonly ILogger<CompetitionChallengesController> logger;
        public CompetitionChallengesController(ICompChallengesRepository _repository,ILogger<CompetitionChallengesController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
             // get
        [HttpGet]
        public async Task<IEnumerable<CompChallengeDto>> GetCompetitionChallengesAsync()
        {
            var competitionchallenges =  (await repository.GetCompChallengesAsync()).Select(CC => CC.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {competitionchallenges.Count()} competitionchallenges");
            return competitionchallenges;
        }
        //get competitionchallenges/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetCompetitionChallengeAsync))]

        public async Task<ActionResult<CompChallengeDto>> GetCompetitionChallengeAsync(Guid id)
        {
            var competitionchallenge = await repository.GetCompChallengeAsync(id);
            if (competitionchallenge is null)
            {
                return NotFound();
            }
            return competitionchallenge.AsDto(); 
        }
        // POST /competitionchallenges
        [HttpPost]
        public async Task<ActionResult<CompChallengeDto>> CreateCompetitionChallengeAsync (CreateCompChallengeDto competitionchallengeDto)
        {
                CompetitionChallenge competitionchallenge = new ()
                {
                    Id = Guid.NewGuid(),

                };
                await repository.CreateCompChallengeAsync(competitionchallenge);
                return CreatedAtAction(nameof(GetCompetitionChallengeAsync),new {id = competitionchallenge.Id}, competitionchallenge.AsDto());

        } 
        //Task<ActionResult<List<CompetitionChallengeDto>>> 
       // POST /competitionchallenges
        [Route("competitionchallenge")]
        [HttpPost]

        public async Task<int> CreateCompetitionChallengesByCompetitionChallengeAsync ([FromBody] CreateChallengesBySeason competitionchallengeDtoList)
        {
            Console.WriteLine(competitionchallengeDtoList.challenges[0].BakerName);
            //CreateByCompetitionChallenge myDeserializedClass = JsonSerializer.Deserialize<CreateByCompetitionChallenge>(competitionchallengeDtoList);
            List<CompetitionChallenge> competitionchallengeslist= new List<CompetitionChallenge>();
                 foreach (var item in competitionchallengeDtoList.challenges)
                 {
                CompetitionChallenge competitionchallenge = new ()
                {
                    Id = Guid.NewGuid(),
                    //Name = item.CompetitionChallengeName,
                    //Age = int.Parse(item.Age),
                    //Hometown= item.Hometown,
                    //Occupation= item.Occupation,
                    //CreatedDate = DateTimeOffset.UtcNow
                };
                competitionchallengeslist.Add(competitionchallenge);
                 }
               await repository.CreateMultipleCompChallengesAsync(competitionchallengeslist);
               // return CreatedAtAction(nameof(GetCompetitionChallengeAsync),new {id = competitionchallenge.Id}, competitionchallenge.AsDto());
              return 4;
        } 
      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateCompetitionChallengeAsync(Guid id, UpdateCompChallengeDto compchallengeDto)
      {
          var existingCompetitionChallenge = await repository.GetCompChallengeAsync(id);
          if (existingCompetitionChallenge is null)
          {
              return NotFound();
          }
          CompetitionChallenge updateCompetitionChallenge = existingCompetitionChallenge with 
          {

          };
          await repository.UpdateCompChallengeAsync(updateCompetitionChallenge);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteCompetitionChallengeAsync(Guid id)
      {
          var existingCompetitionChallenge = await repository.GetCompChallengeAsync(id);
          if (existingCompetitionChallenge is null)
          {
              return NotFound();
          }

          await repository.DeleteCompChallengeAsync(id);
          return NoContent();
      }

    }
    
}