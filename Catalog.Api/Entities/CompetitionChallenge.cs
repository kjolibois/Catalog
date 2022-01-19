using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities
{
    public record CompetitionChallenge: BaseEntity
{
   public string ShowstopperChallenge {get;set;}
   public string TechnicalChallenge {get;set;}
   public string SignatureChallenge {get;set;}
   public string WeekTheme {get;set;}
   public int WeekNumber {get;set;}
   public string SeasonId {get;set;}

}

    
}