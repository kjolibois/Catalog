using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface ICompResultsRepository
    {
        Task<IEnumerable<CompetitionResult>> GetCompResultsAsync();
        Task<CompetitionResult> GetCompResultAsync(Guid id);
        Task CreateCompResultAsync(CompetitionResult compresult);
        Task CreateMultipleCompResultsAsync( List<CompetitionResult> compresults);
        Task UpdateCompResultAsync(CompetitionResult compresult);
        Task DeleteCompResultAsync (Guid id);
    }
}