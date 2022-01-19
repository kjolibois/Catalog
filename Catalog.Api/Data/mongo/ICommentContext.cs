using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public interface ICommentContext
    {
        public IMongoCollection<Baker> bakerCollection {get;}
    }
}