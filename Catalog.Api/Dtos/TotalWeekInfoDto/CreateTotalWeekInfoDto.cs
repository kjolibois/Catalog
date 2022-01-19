using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record CreateWeekDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(1,1000)]
        public decimal Price { get; init; }
    }
}