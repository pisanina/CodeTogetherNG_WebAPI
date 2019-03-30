using System;
using System.Collections.Generic;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class Technology
    {
        public Technology()
        {
            ProjectTechnology = new HashSet<ProjectTechnology>();
            UserTechnologyLevel = new HashSet<UserTechnologyLevel>();
        }

        public int Id { get; set; }
        public string TechName { get; set; }

        public ICollection<ProjectTechnology> ProjectTechnology { get; set; }
        public ICollection<UserTechnologyLevel> UserTechnologyLevel { get; set; }
    }
}
