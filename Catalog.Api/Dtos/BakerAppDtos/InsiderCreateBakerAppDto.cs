using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record InsiderCreateBakerAppDto
    {
        public Guid BakerId {get;set;}
        public Guid? SeasonId {get;set;}
        public bool SpecialAppearance {get;set;}
        public Guid? SpecialsId {get;set;}
        public int AppearanceYear {get;set;}
    }
}