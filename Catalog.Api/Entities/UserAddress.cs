using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Entities
{
    public class UserAddress : Address
    {
        public int Id { get; set; }
        public int MyProperty { get; set; }
    }
}