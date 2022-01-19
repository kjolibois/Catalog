using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Entities
{
    public record BaseEntity
    {
        public Guid Id { get; set;}
        public DateTimeOffset  ModifiedDate { get; set;}
        public DateTimeOffset  CreatedDate { get; set; }
    }
}