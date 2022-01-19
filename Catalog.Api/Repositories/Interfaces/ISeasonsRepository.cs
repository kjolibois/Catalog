using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface ISeasonsRepository
    {
        Task<IEnumerable<Season>> GetSeasonsAsync();
        Task<Season> GetSeasonAsync(Guid id);
        Task CreateSeasonAsync(Season season);
        Task CreateMultipleSeasonsAsync( List<Season> seasons);
        Task UpdateSeasonAsync(Season season);
        Task DeleteSeasonAsync (Guid id);
    }
}