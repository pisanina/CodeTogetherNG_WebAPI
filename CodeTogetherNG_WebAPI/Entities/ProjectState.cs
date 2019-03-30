using System;
using System.Collections.Generic;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class ProjectState
    {
        public ProjectState()
        {
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string State { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
