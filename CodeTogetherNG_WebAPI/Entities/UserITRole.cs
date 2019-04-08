using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.Entities
{
    public class UserITRole
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }

        public ITRole Role { get; set; }
        public AspNetUsers User { get; set; }
    }
}
