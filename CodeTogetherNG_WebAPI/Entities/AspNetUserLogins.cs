using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class AspNetUserLogins : IdentityUserLogin<string>
    {
        public AspNetUsers User { get; set; }
    }
}
