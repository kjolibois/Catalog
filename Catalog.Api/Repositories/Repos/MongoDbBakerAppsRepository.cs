using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbBakerAppsRepository : IBakerAppsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "bakers";
        private readonly IMongoCollection<BakerAppearance> bakersCollection;
        private readonly FilterDefinitionBuilder<BakerAppearance> filterBuilder = Builders<BakerAppearance>.Filter;
        public MongoDbBakerAppsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            bakersCollection = database.GetCollection<BakerAppearance>(collectionName);
        }

        public async Task CreateBakerAppAsync(BakerAppearance baker)
        {
            await bakersCollection.InsertOneAsync(baker);
        }

        public async Task CreateMultipleBakerAppsAsync(List<BakerAppearance> bakers)
        { 
            Console.WriteLine(bakers[0]);
           await bakersCollection.InsertManyAsync(bakers);  
        }

        public async Task DeleteBakerAppAsync(Guid id)
        {
            var filter = filterBuilder.Eq(ExistingBakerApp => ExistingBakerApp.Id, id);
            await bakersCollection.DeleteOneAsync(filter);     
        }

        public async Task<BakerAppearance> GetBakerAppAsync(Guid id)
        {
            var filter = filterBuilder.Eq(baker => baker.Id, id);
            return await bakersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<BakerAppearance>> GetBakerAppsAsync()
        {
            return await bakersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateBakerAppAsync(BakerAppearance baker)
        {
           var filter = filterBuilder.Eq(ExistingBakerApp => ExistingBakerApp.Id, baker.Id);
           await bakersCollection.ReplaceOneAsync(filter,baker);        
        }
    }
}