using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record CreateBakerAppDto
    {
        public string BakerName {get;set;}
        public int UKSeriesNumber {get;set;}
        public bool SpecialAppearance {get;set;}
        public string? SpecialsName {get;set;}
        public int AppearanceYear {get;set;}
    }
}