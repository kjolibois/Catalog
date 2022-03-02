using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Catalog.Api.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }

        
    }
}