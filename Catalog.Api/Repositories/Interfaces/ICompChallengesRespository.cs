using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface ICompChallengesRepository
    {
        Task<IEnumerable<CompetitionChallenge>> GetCompChallengesAsync();
        Task<CompetitionChallenge> GetCompChallengeAsync(Guid id);
        Task CreateCompChallengeAsync(CompetitionChallenge compchallenge);
        Task CreateMultipleCompChallengesAsync( List<CompetitionChallenge> compchallenges);
        Task UpdateCompChallengeAsync(CompetitionChallenge compchallenge);
        Task DeleteCompChallengeAsync (Guid id);
    }
}