using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbSeasonsRepository : ISeasonsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "seasons";
        private readonly IMongoCollection<Season> seasonsCollection;
        private readonly FilterDefinitionBuilder<Season> filterBuilder = Builders<Season>.Filter;
        public MongoDbSeasonsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            seasonsCollection = database.GetCollection<Season>(collectionName);
        }

        public async Task CreateSeasonAsync(Season season)
        {
            await seasonsCollection.InsertOneAsync(season);
        }

        public async Task CreateMultipleSeasonsAsync(List<Season> seasons)
        { 
            Console.WriteLine(seasons[0]);
           await seasonsCollection.InsertManyAsync(seasons);  
        }

        public async Task DeleteSeasonAsync(Guid id)
        {
            var filter = filterBuilder.Eq(ExistingSeason => ExistingSeason.Id, id);
            await seasonsCollection.DeleteOneAsync(filter);     
        }

        public async Task<Season> GetSeasonAsync(Guid id)
        {
            var filter = filterBuilder.Eq(season => season.Id, id);
            return await seasonsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Season>> GetSeasonsAsync()
        {
            return await seasonsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateSeasonAsync(Season season)
        {
           var filter = filterBuilder.Eq(ExistingSeason => ExistingSeason.Id, season.Id);
           await seasonsCollection.ReplaceOneAsync(filter,season);        
        }
    }
}