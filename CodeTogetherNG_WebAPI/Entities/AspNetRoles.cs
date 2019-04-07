using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class AspNetRoles : IdentityRole
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }

        public ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
