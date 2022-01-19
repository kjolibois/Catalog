using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbCompChallengesRepository : ICompChallengesRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "compchallenges";
        private readonly IMongoCollection<CompetitionChallenge> compchallengesCollection;
        private readonly FilterDefinitionBuilder<CompetitionChallenge> filterBuilder = Builders<CompetitionChallenge>.Filter;
        public MongoDbCompChallengesRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            compchallengesCollection = database.GetCollection<CompetitionChallenge>(collectionName);
        }

        public async Task CreateCompChallengeAsync(CompetitionChallenge compchallenge)
        {
            await compchallengesCollection.InsertOneAsync(compchallenge);
        }

        public async Task CreateMultipleCompChallengesAsync(List<CompetitionChallenge> compchallenges)
        { 
            Console.WriteLine(compchallenges[0]);
           await compchallengesCollection.InsertManyAsync(compchallenges);  
        }

        public async Task DeleteCompChallengeAsync(Guid id)
        {
            var filter = filterBuilder.Eq(ExistingCompChallenge => ExistingCompChallenge.Id, id);
            await compchallengesCollection.DeleteOneAsync(filter);     
        }

        public async Task<CompetitionChallenge> GetCompChallengeAsync(Guid id)
        {
            var filter = filterBuilder.Eq(compchallenge => compchallenge.Id, id);
            return await compchallengesCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CompetitionChallenge>> GetCompChallengesAsync()
        {
            return await compchallengesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateCompChallengeAsync(CompetitionChallenge compchallenge)
        {
           var filter = filterBuilder.Eq(ExistingCompChallenge => ExistingCompChallenge.Id, compchallenge.Id);
           await compchallengesCollection.ReplaceOneAsync(filter,compchallenge);        
        }
    }
}