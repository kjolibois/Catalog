using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record SeasonDto
    {
        public Guid Id { get; init;}

        public int OriginalAiringYear { get; set; }
        
        [Range(1,200)]
        public int UkSeriesNumber {get;set;}
        public string? NetflixCollection { get; set; }
        public int? PBSSeason {get;set;}


    } 
}