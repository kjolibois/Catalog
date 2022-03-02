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
    public class SpecialsController : ControllerBase
    {
        private readonly ISpecialsRepository repository;
        private readonly ILogger<SpecialsController> logger;
        public SpecialsController(ISpecialsRepository _repository,ILogger<SpecialsController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
             // get
        [HttpGet]
        public async Task<IEnumerable<SpecialDto>> GetSpecialsAsync()
        {
            var specials = (await repository.GetSpecialsAsync()).Select(baker => baker.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {specials.Count()} specials");
            return specials;
        }
        //get specials/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetSpecialAsync))]

        public async Task<ActionResult<SpecialDto>> GetSpecialAsync(Guid id)
        {
            var special = await repository.GetSpecialAsync(id);
            if (special is null)
            {
                return NotFound();
            }
            return special.AsDto(); 
        }
        // POST /specials
        [HttpPost]
        public async Task<ActionResult<SpecialDto>> CreateSpecialAsync (CreateSpecialDto specialDto)
        {
                Special special = new ()
                {
                    Id = Guid.NewGuid(),
                    //Name = specialDto.SpecialName,

                    CreatedDate = DateTimeOffset.UtcNow
                };
                await repository.CreateSpecialAsync(special);
                return CreatedAtAction(nameof(GetSpecialAsync),new {id = special.Id}, special.AsDto());

        } 
        /*
        //Task<ActionResult<List<SpecialDto>>> 
       // POST /specials
        [Route("special")]
        [HttpPost]

        public async Task<int> CreateSpecialsBySpecialAsync ([FromBody] CreateBySpecial specialDtoList)
        {
            Console.WriteLine(specialDtoList.specials[0].SpecialName);
            //CreateBySpecial myDeserializedClass = JsonSerializer.Deserialize<CreateBySpecial>(specialDtoList);
            List<Special> specialslist= new List<Special>();
                 foreach (var item in specialDtoList.specials)
                 {
                Special special = new ()
                {
                    Id = Guid.NewGuid(),
                    Name = item.SpecialName,
                    Age = int.Parse(item.Age),
                    Hometown= item.Hometown,
                    Occupation= item.Occupation,
                    CreatedDate = DateTimeOffset.UtcNow
                };
                specialslist.Add(special);
                 }
               await repository.CreateMultipleSpecialsAsync(specialslist);
               // return CreatedAtAction(nameof(GetSpecialAsync),new {id = special.Id}, special.AsDto());
              return 4;
        } 
        */
      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateSpecialAsync(Guid id, UpdateSpecialDto specialDto)
      {
          var existingSpecial = await repository.GetSpecialAsync(id);
          if (existingSpecial is null)
          {
              return NotFound();
          }
          Special updateSpecial = existingSpecial with 
          {
              //Name = specialDto.Name,

          };
          await repository.UpdateSpecialAsync(updateSpecial);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteSpecialAsync(Guid id)
      {
          var existingSpecial = await repository.GetSpecialAsync(id);
          if (existingSpecial is null)
          {
              return NotFound();
          }

          await repository.DeleteSpecialAsync(id);
          return NoContent();
      }

    }
    
}