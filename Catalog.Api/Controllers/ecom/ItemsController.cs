using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController: ControllerBase
    {
        private readonly IItemsRepository repository;
        private readonly ILogger<ItemsController> logger;
        public ItemsController(IItemsRepository _repository,ILogger<ItemsController> _logger)
        {
            repository = _repository;
            logger=_logger;

        }
        // get
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string? name = null)
        {
            var items = (await repository.GetItemsAsync()).Select(item => new ItemDto (
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.CreatedDate));
                 logger.LogInformation(name);
            if (!string.IsNullOrWhiteSpace(name)){
                items = items.Where(item => item.Name.Contains(name,StringComparison.OrdinalIgnoreCase));

            }
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");
            return items;
        }
        //get items/{id}
        [HttpGet("{id}")]
        [ActionName(nameof(GetItemAsync))]

        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto(); 
        }
        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync (CreateItemDto itemDto)
        {
                Item item = new ()
                {
                    Id = Guid.NewGuid(),
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Description = itemDto.Description,
                    CreatedDate= DateTimeOffset.UtcNow
                };
                await repository.CreateItemAsync(item);
                return CreatedAtAction(nameof(GetItemAsync),new {id = item.Id}, item.AsDto());

        } 

      [HttpPut("{id}")]
      public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
      {
          Item existingItem = await repository.GetItemAsync(id);
          if (existingItem is null)
          {
              return NotFound();
          }
          existingItem.Name = itemDto.Name;
          existingItem.Price = itemDto.Price;
       // record   Item updateItem = existingItem with 
         // {
           //   Name = itemDto.Name,
             // Price = itemDto.Price
          //};
          await repository.UpdateItemAsync(existingItem);
          return NoContent();
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult> DeleteItemAsync(Guid id)
      {
          var existingItem = await repository.GetItemAsync(id);
          if (existingItem is null)
          {
              return NotFound();
          }

          await repository.DeleteItemAsync(id);
          return NoContent();
      }


    }
}