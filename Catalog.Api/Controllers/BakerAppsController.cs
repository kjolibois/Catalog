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
    public class BakerAppearancesController : ControllerBase
    {
        private readonly IBakerAppsRepository repository;
        private readonly ILogger<BakerAppearancesController> logger;
        public BakerAppearancesController(IBakerAppsRepository _repository,ILogger<BakerAppearancesController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
             // get
        [HttpGet]
        public async Task<IEnumerable<BakerAppDto>> GetBakerAppsAsync()
        {
            var bakerAppearances = (await repository.GetBakerAppsAsync()).Select(bakerAppearance => bakerAppearance.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {bakerAppearances.Count()} bakerAppearances");
            return bakerAppearances;
        }
        //get bakerAppearances/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetBakerAppAsync))]

        public async Task<ActionResult<BakerAppDto>> GetBakerAppAsync(Guid id)
        {
            var bakerAppearance = await repository.GetBakerAppAsync(id);
            if (bakerAppearance is null)
            {
                return NotFound();
            }
            return bakerAppearance.AsDto(); 
        }
        // POST /bakerAppearances
        [HttpPost]
        public async Task<ActionResult<BakerAppDto>> CreateBakerAppAsync (CreateBakerAppDto bakerAppDto)
        {
                BakerAppearance bakerAppearance = new ()
                {
                    Id = Guid.NewGuid(),
                    //BakerId = bakerAppDto.BakerName find id
                    //SeasonId = bakerAppDto.UKSeriesNumber
                    SpecialAppearance = bakerAppDto.SpecialAppearance,
                    //SpecialsId = bakerAppDto.SpecialsName find
                    AppearanceYear= bakerAppDto.AppearanceYear

                };
                await repository.CreateBakerAppAsync(bakerAppearance);
                return CreatedAtAction(nameof(GetBakerAppAsync),new {id = bakerAppearance.Id}, bakerAppearance.AsDto());

        } 
        
      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateBakerAppAsync(Guid id, UpdateBakerAppDto bakerAppDto)
      {
          var existingBakerApp = await repository.GetBakerAppAsync(id);
          if (existingBakerApp is null)
          {
              return NotFound();
          }
          BakerAppearance updateBakerApp = existingBakerApp with 
          {
                    
                    //BakerId = bakerAppDto.BakerName find id
                    //SeasonId = bakerAppDto.UKSeriesNumber
                    SpecialAppearance = bakerAppDto.SpecialAppearance,
                    //SpecialsId = bakerAppDto.SpecialsName find
                    AppearanceYear= bakerAppDto.AppearanceYear
          };
          await repository.UpdateBakerAppAsync(updateBakerApp);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteBakerAppAsync(Guid id)
      {
          var existingBakerApp = await repository.GetBakerAppAsync(id);
          if (existingBakerApp is null)
          {
              return NotFound();
          }

          await repository.DeleteBakerAppAsync(id);
          return NoContent();
      }

    }
    
}