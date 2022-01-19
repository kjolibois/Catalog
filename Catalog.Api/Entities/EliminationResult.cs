using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities
{
    public record EliminationResult: BaseEntity
    {
       

        [ForeignKey("Baker")]
        public Guid BakerId { get; set; }
        [ForeignKey("SeasonId")]
        public Guid SeasonId {get;set;}
        public string ScrapeDecisionFactor {get;set;}
        public int WeekNumber {get;set;}
        public string Result {get;set;}
    }
}