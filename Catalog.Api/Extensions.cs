using Catalog.Api.Dtos;
using Catalog.Api.Entities;
namespace Catalog.Api
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(                item.Id,
                 item.Name,
                 item.Description,
                 item.Price,
                 item.CreatedDate
            );
        }
        public static BakerAppDto AsDto(this BakerAppearance bakerAppearance)
        {
            return new BakerAppDto
            {
                Id  = bakerAppearance.Id,
     
            };
        } 
        public static BakerDto AsDto(this Baker baker)
        {
            return new BakerDto
            {
                Id = baker.Id,
                Name = baker.Name,
                Hometown = baker.Hometown,
                CreatedDate = baker.CreatedDate
            };
        }
        public static CompChallengeDto AsDto(this CompetitionChallenge compChallenge)
        {
            return new CompChallengeDto
            {
                Id  = compChallenge.Id,
     
            };
        }
        public static CompResultDto AsDto(this CompetitionResult placeholder)
        {
            return new CompResultDto
            {
                Id  = placeholder.Id,
     
            };
        } 
        public static SeasonDto AsDto(this Season season)
        {
            return new SeasonDto
            {
                Id  = season.Id,
                OriginalAiringYear = season.OriginalAiringYear,
                UkSeriesNumber = season.UkSeriesNumber,
                NetflixCollection = season.NetflixCollection,
                PBSSeason = season.PBSSeason,
    
            };
        } 
        public static TotalWeekInfoDto AsDto(this TotalWeekInfo weekinfo)
        {
            return new TotalWeekInfoDto
            {
                Id  = weekinfo.Id,
     
            };
        } 
        public static SpecialDto AsDto(this Special special)
        {
            return new SpecialDto
            {
                Id  = special.Id,
     
            };
        }
/*
        public static PlaceholderDto AsDto(this Placeholder placeholder)
        {
            return new PlaceholderDto
            {
                Id  = placeholder.Id,
     
            };
        } 
        public static PlaceholderDto AsDto(this Placeholder placeholder)
        {
            return new PlaceholderDto
            {
                Id  = placeholder.Id,
     
            };
        } 
*/
        
    }
}