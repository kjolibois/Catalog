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
    public class BakersController : ControllerBase
    {
        private readonly IBakersRepository repository;
        private readonly ILogger<BakersController> logger;
        public BakersController(IBakersRepository _repository,ILogger<BakersController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
             // get
        [HttpGet]
        public async Task<IEnumerable<BakerDto>> GetBakersAsync()
        {
            var bakers = (await repository.GetBakersAsync()).Select(baker => baker.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {bakers.Count()} bakers");
            return bakers;
        }
        //get bakers/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetBakerAsync))]

        public async Task<ActionResult<BakerDto>> GetBakerAsync(Guid id)
        {
            var baker = await repository.GetBakerAsync(id);
            if (baker is null)
            {
                return NotFound();
            }
            return baker.AsDto(); 
        }
        // POST /bakers
        [HttpPost]
        public async Task<ActionResult<BakerDto>> CreateBakerAsync (CreateBakerDto bakerDto)
        {
                Baker baker = new ()
                {
                    Id = Guid.NewGuid(),
                    Name = bakerDto.BakerName,
                    Age = int.Parse(bakerDto.Age),
                    Hometown= bakerDto.Hometown,
                    Occupation= bakerDto.Occupation,
                    CreatedDate = DateTimeOffset.UtcNow
                };
                await repository.CreateBakerAsync(baker);
                return CreatedAtAction(nameof(GetBakerAsync),new {id = baker.Id}, baker.AsDto());

        } 
        //Task<ActionResult<List<BakerDto>>> 
       // POST /bakers
        [Route("season")]
        [HttpPost]

        public async Task<int> CreateBakersBySeasonAsync ([FromBody] CreateBySeason bakerDtoList)
        {
            Console.WriteLine(bakerDtoList.bakers[0].BakerName);
            //CreateBySeason myDeserializedClass = JsonSerializer.Deserialize<CreateBySeason>(bakerDtoList);
            List<Baker> bakerslist= new List<Baker>();
                 foreach (var item in bakerDtoList.bakers)
                 {
                Baker baker = new ()
                {
                    Id = Guid.NewGuid(),
                    Name = item.BakerName,
                    Age = int.Parse(item.Age),
                    Hometown= item.Hometown,
                    Occupation= item.Occupation,
                    CreatedDate = DateTimeOffset.UtcNow
                };
                bakerslist.Add(baker);
                 }
               await repository.CreateMultipleBakersAsync(bakerslist);
               // return CreatedAtAction(nameof(GetBakerAsync),new {id = baker.Id}, baker.AsDto());
              return 4;
        } 
      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateBakerAsync(Guid id, UpdateBakerDto bakerDto)
      {
          var existingBaker = await repository.GetBakerAsync(id);
          if (existingBaker is null)
          {
              return NotFound();
          }
          Baker updateBaker = existingBaker with 
          {
              Name = bakerDto.Name,
              Age = bakerDto.Age,
              Occupation = bakerDto.Occupation, 
              Hometown = bakerDto.Hometown
          };
          await repository.UpdateBakerAsync(updateBaker);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteBakerAsync(Guid id)
      {
          var existingBaker = await repository.GetBakerAsync(id);
          if (existingBaker is null)
          {
              return NotFound();
          }

          await repository.DeleteBakerAsync(id);
          return NoContent();
      }

    }
    
}