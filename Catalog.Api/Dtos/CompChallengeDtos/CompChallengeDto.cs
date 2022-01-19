using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record CompChallengeDto
    {   
        public Guid Id { get; set;}
        public string Name { get; set; }
        
        [Range(1,200)]
        public int Age {get;set;}
        public string Hometown { get; set; }
        public string Occupation {get;set;}
        public DateTimeOffset  CreatedDate { get; set; }

    } 
}