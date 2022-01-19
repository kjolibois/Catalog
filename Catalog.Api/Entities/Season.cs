using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities
{
    public record Season: BaseEntity
    {
  
        public int OriginalAiringYear { get; set; }
        
        [Range(1,200)]
        public int UkSeriesNumber {get;set;}
        public string? NetflixCollection { get; set; }
        public int? PBSSeason {get;set;}

        
    }
}