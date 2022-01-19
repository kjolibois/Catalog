using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IBakersRepository
    {
        Task<IEnumerable<Baker>> GetBakersAsync();
        Task<Baker> GetBakerAsync(Guid id);
        Task CreateBakerAsync(Baker baker);
        Task CreateMultipleBakersAsync( List<Baker> bakers);
        Task UpdateBakerAsync(Baker baker);
        Task DeleteBakerAsync (Guid id);
    }
}