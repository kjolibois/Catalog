using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbBakersRepository : IBakersRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "bakers";
        private readonly IMongoCollection<Baker> bakersCollection;
        private readonly FilterDefinitionBuilder<Baker> filterBuilder = Builders<Baker>.Filter;
        public MongoDbBakersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            bakersCollection = database.GetCollection<Baker>(collectionName);
        }

        public async Task CreateBakerAsync(Baker baker)
        {
            await bakersCollection.InsertOneAsync(baker);
        }

        public async Task CreateMultipleBakersAsync(List<Baker> bakers)
        { 
            Console.WriteLine(bakers[0]);
           await bakersCollection.InsertManyAsync(bakers);  
        }

        public async Task DeleteBakerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(ExistingBaker => ExistingBaker.Id, id);
            await bakersCollection.DeleteOneAsync(filter);     
        }

        public async Task<Baker> GetBakerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(baker => baker.Id, id);
            return await bakersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Baker>> GetBakersAsync()
        {
            return await bakersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateBakerAsync(Baker baker)
        {
           var filter = filterBuilder.Eq(ExistingBaker => ExistingBaker.Id, baker.Id);
           await bakersCollection.ReplaceOneAsync(filter,baker);        
        }
    }
}