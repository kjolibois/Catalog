using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Entities
{
    public record BakerAppearance: BaseEntity
    {
         public Guid BakerId {get;set;}
      public Guid? SeasonId {get;set;}
      public bool SpecialAppearance {get;set;}
      public Guid? SpecialsId {get;set;}
      public int AppearanceYear {get;set;}

    }
}