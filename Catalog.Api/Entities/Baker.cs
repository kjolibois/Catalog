using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Entities
{
    public record Baker: BaseEntity
    {
        public string Name { get; set; }
        
        [Range(1,200)]
        public int Age {get;set;}
        public string Hometown { get; set; }
        public string Occupation {get;set;}


    }
}