using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbSpecialsRepository : ISpecialsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "specials";
        private readonly IMongoCollection<Special> specialsCollection;
        private readonly FilterDefinitionBuilder<Special> filterBuilder = Builders<Special>.Filter;
        public MongoDbSpecialsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            specialsCollection = database.GetCollection<Special>(collectionName);
        }

        public async Task CreateSpecialAsync(Special special)
        {
            await specialsCollection.InsertOneAsync(special);
        }

        public async Task CreateMultipleSpecialsAsync(List<Special> specials)
        { 
            Console.WriteLine(specials[0]);
           await specialsCollection.InsertManyAsync(specials);  
        }

        public async Task DeleteSpecialAsync(Guid id)
        {
            var filter = filterBuilder.Eq(ExistingSpecial => ExistingSpecial.Id, id);
            await specialsCollection.DeleteOneAsync(filter);     
        }

        public async Task<Special> GetSpecialAsync(Guid id)
        {
            var filter = filterBuilder.Eq(special => special.Id, id);
            return await specialsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Special>> GetSpecialsAsync()
        {
            return await specialsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateSpecialAsync(Special special)
        {
           var filter = filterBuilder.Eq(ExistingSpecial => ExistingSpecial.Id, special.Id);
           await specialsCollection.ReplaceOneAsync(filter,special);        
        }
    }
}