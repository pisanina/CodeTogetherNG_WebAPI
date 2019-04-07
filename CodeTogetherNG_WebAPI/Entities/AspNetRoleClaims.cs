using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class AspNetRoleClaims : IdentityRoleClaim<string>
    {
        public AspNetRoles Role { get; set; }
    }
}
