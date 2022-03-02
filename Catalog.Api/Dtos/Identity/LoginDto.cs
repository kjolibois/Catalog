using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Dtos.Identity
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}