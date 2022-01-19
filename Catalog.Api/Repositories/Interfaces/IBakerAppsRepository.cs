using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IBakerAppsRepository
    {
        Task<IEnumerable<BakerAppearance>> GetBakerAppsAsync();
        Task<BakerAppearance> GetBakerAppAsync(Guid id);
        Task CreateBakerAppAsync(BakerAppearance bakerapp);
        Task CreateMultipleBakerAppsAsync( List<BakerAppearance> bakerapps);
        Task UpdateBakerAppAsync(BakerAppearance bakerapp);
        Task DeleteBakerAppAsync (Guid id);
    }
}