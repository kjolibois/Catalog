using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbCompResultsRepository : ICompResultsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "compResults";
        private readonly IMongoCollection<CompetitionResult> compResultsCollection;
        private readonly FilterDefinitionBuilder<CompetitionResult> filterBuilder = Builders<CompetitionResult>.Filter;
        public MongoDbCompResultsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            compResultsCollection = database.GetCollection<CompetitionResult>(collectionName);
        }

        public async Task CreateCompResultAsync(CompetitionResult compResult)
        {
            await compResultsCollection.InsertOneAsync(compResult);
        }

        public async Task CreateMultipleCompResultsAsync(List<CompetitionResult> compResults)
        { 
            Console.WriteLine(compResults[0]);
           await compResultsCollection.InsertManyAsync(compResults);  
        }

        public async Task DeleteCompResultAsync(Guid id)
        {
            var filter = filterBuilder.Eq(ExistingCompResult => ExistingCompResult.Id, id);
            await compResultsCollection.DeleteOneAsync(filter);     
        }

        public async Task<CompetitionResult> GetCompResultAsync(Guid id)
        {
            var filter = filterBuilder.Eq(compResult => compResult.Id, id);
            return await compResultsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CompetitionResult>> GetCompResultsAsync()
        {
            return await compResultsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateCompResultAsync(CompetitionResult compResult)
        {
           var filter = filterBuilder.Eq(ExistingCompResult => ExistingCompResult.Id, compResult.Id);
           await compResultsCollection.ReplaceOneAsync(filter,compResult);        
        }
    }
}