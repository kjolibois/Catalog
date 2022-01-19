namespace Catalog.Api.Dtos
{
    public record BakerAppDto
    {
        public Guid Id { get; init;}    
        public Guid BakerId {get;set;}
        public Guid? SeasonId {get;set;}
        public bool SpecialAppearance {get;set;}
        public Guid? SpecialsId {get;set;}
        public int AppearanceYear {get;set;}
    } 
}