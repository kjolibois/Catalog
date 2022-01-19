using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using MongoDB.Driver;

namespace Catalog.Data
{
    public class PrepDb
    {

    public static void PrepPopulation(string connectionString)
      {
           
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("catalog");
            IMongoCollection<Season>  seasonCollection= database.GetCollection<Season>("seasons");
            SeedSeasons(seasonCollection,false);
      }
        
        private static IEnumerable<Baker> GetBakers() {
         return new List<Baker>(){
             new Baker(){ CreatedDate= DateTimeOffset.Now, Name= "test", Hometown="testtown", Age= 32, Occupation= "C# Dev" },
             new Baker(){ CreatedDate= DateTimeOffset.Now, Name= "test2", Hometown="testtown2", Age= 50, Occupation= "farmer" }
         };
        }
        private static IEnumerable<Season> GetSeasons() {
         return new List<Season>(){
             new Season()
             {   
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2010, 
                 UkSeriesNumber= 1, 
                 NetflixCollection=null, 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2011, 
                 UkSeriesNumber= 2, 
                 NetflixCollection=null, 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2012, 
                 UkSeriesNumber= 3, 
                 NetflixCollection="The Beginnings", 
                 PBSSeason= 5, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2013, 
                 UkSeriesNumber= 4, 
                 NetflixCollection="2", 
                 PBSSeason= 2 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2014, 
                 UkSeriesNumber= 5, 
                 NetflixCollection="1", 
                 PBSSeason= 1, 
             },
             new Season()
             { 

                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2015, 
                 UkSeriesNumber= 6, 
                 NetflixCollection="3", 
                 PBSSeason= 3, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2016, 
                 UkSeriesNumber= 7, 
                 NetflixCollection="4", 
                 PBSSeason= 4, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2017, 
                 UkSeriesNumber= 8, 
                 NetflixCollection="5", 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(), 
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2018, 
                 UkSeriesNumber= 9, 
                 NetflixCollection="6", 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(), 
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2019, 
                 UkSeriesNumber= 10, 
                 NetflixCollection="7", 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2020, 
                 UkSeriesNumber= 11, 
                 NetflixCollection="8", 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2021, 
                 UkSeriesNumber= 12, 
                 NetflixCollection="9", 
                 PBSSeason= null, 
             },
             new Season()
             { 
                 Id =Guid.NewGuid(),
                 CreatedDate= DateTimeOffset.Now, 
                 OriginalAiringYear= 2022, 
                 UkSeriesNumber= 13, 
                 NetflixCollection="10", 
                 PBSSeason= null, 
             },
    
         };
        }
        private static void SeedData(IMongoCollection<Baker> bakerCollection, bool isProd )
      {
         /* if(isProd)
          {
              Console.WriteLine("--> Attempting to apply migrations..");
              try{
              context.Database.Migrate();
              }
              catch(Exception ex)
              {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
              }
          }*/
          var bakersExist = bakerCollection.Find(p => true).Any();
          if(!bakersExist)
          {
              Console.WriteLine("--> Seeding Data...");
              bakerCollection.InsertManyAsync(GetBakers());
              
          }
          else
          {
              Console.WriteLine("--> We already have data");
          }
      }
     private static void SeedSeasons(IMongoCollection<Season> seasonCollection, bool isProd )
      {
         /* if(isProd)
          {
              Console.WriteLine("--> Attempting to apply migrations..");
              try{
              context.Database.Migrate();
              }
              catch(Exception ex)
              {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
              }
          }*/
          var seasonsExist = seasonCollection.Find(p => true).Any();
          if(!seasonsExist)
          {
              Console.WriteLine("--> Seeding Data...");
              seasonCollection.InsertManyAsync(GetSeasons());
              
          }
          else
          {
              Console.WriteLine("--> We already have data");
          }
      }

    }
}