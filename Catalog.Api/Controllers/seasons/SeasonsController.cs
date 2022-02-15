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
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonsRepository repository;
        private readonly ILogger<SeasonsController> logger;
        public SeasonsController(ISeasonsRepository _repository,ILogger<SeasonsController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
             // get
        [HttpGet]
        public async Task<IEnumerable<SeasonDto>> GetSeasonsAsync()
        {
            var seasons = (await repository.GetSeasonsAsync()).Select(baker => baker.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {seasons.Count()} seasons");
            return seasons;
        }
        //get seasons/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetSeasonAsync))]

        public async Task<ActionResult<SeasonDto>> GetSeasonAsync(Guid id)
        {
            var season = await repository.GetSeasonAsync(id);
            if (season is null)
            {
                return NotFound();
            }
            return season.AsDto(); 
        }
        // POST /seasons
        [HttpPost]
        public async Task<ActionResult<SeasonDto>> CreateSeasonAsync (CreateSeasonDto seasonDto)
        {
                Season season = new ()
                {
                    Id = Guid.NewGuid(),
                    //Name = seasonDto.SeasonName,
            
                    CreatedDate = DateTimeOffset.UtcNow
                };
                await repository.CreateSeasonAsync(season);
                return CreatedAtAction(nameof(GetSeasonAsync),new {id = season.Id}, season.AsDto());

        } 
        //Task<ActionResult<List<SeasonDto>>> 
       // POST /seasons
        [Route("season")]
        [HttpPost]
/*
        public async Task<int> CreateSeasonsBySeasonAsync ([FromBody] CreateBySeason seasonDtoList)
        {
            Console.WriteLine(seasonDtoList.seasons[0].SeasonName);
            //CreateBySeason myDeserializedClass = JsonSerializer.Deserialize<CreateBySeason>(seasonDtoList);
            List<Season> seasonslist= new List<Season>();
                 foreach (var item in seasonDtoList.seasons)
                 {
                Season season = new ()
                {
                    Id = Guid.NewGuid(),
                    Name = item.SeasonName,
                    Age = int.Parse(item.Age),
                    Hometown= item.Hometown,
                    Occupation= item.Occupation,
                    CreatedDate = DateTimeOffset.UtcNow
                };
                seasonslist.Add(season);
                 }
               await repository.CreateMultipleSeasonsAsync(seasonslist);
               // return CreatedAtAction(nameof(GetSeasonAsync),new {id = season.Id}, season.AsDto());
              return 4;
        } */
      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateSeasonAsync(Guid id, UpdateSeasonDto seasonDto)
      {
          var existingSeason = await repository.GetSeasonAsync(id);
          if (existingSeason is null)
          {
              return NotFound();
          }
          Season updateSeason = existingSeason with 
          {
             // Name = seasonDto.Name,

          };
          await repository.UpdateSeasonAsync(updateSeason);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteSeasonAsync(Guid id)
      {
          var existingSeason = await repository.GetSeasonAsync(id);
          if (existingSeason is null)
          {
              return NotFound();
          }

          await repository.DeleteSeasonAsync(id);
          return NoContent();
      }

    }
    
}