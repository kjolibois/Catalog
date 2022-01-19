using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface ISpecialsRepository
    {
        Task<IEnumerable<Special>> GetSpecialsAsync();
        Task<Special> GetSpecialAsync(Guid id);
        Task CreateSpecialAsync(Special special);
        Task CreateMultipleSpecialsAsync( List<Special> specials);
        Task UpdateSpecialAsync(Special special);
        Task DeleteSpecialAsync (Guid id);
    }
}