using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record UpdateBakerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,200)]
        public int Age {get;set;}
        [Required]
        public string Hometown { get; set; }
        [Required]
        public string Occupation {get;set;}
    }
}