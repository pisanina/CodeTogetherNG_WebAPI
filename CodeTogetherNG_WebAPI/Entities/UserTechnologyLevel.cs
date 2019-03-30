using System;
using System.Collections.Generic;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class UserTechnologyLevel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? TechnologyId { get; set; }
        public int TechLevel { get; set; }

        public Technology Technology { get; set; }
        public AspNetUsers User { get; set; }
    }
}
