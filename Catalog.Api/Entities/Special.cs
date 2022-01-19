using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Entities
{
    public record Special : BaseEntity
    {
        public string SpecialName {get;set;}
        public string OriginalAiringYear {get;set;}

    }
}