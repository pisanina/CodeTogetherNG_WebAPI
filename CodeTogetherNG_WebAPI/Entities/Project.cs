using System;
using System.Collections.Generic;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class Project
    {
        public Project()
        {
            ProjectMember = new HashSet<ProjectMember>();
            ProjectTechnology = new HashSet<ProjectTechnology>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string OwnerId { get; set; }
        public bool NewMembers { get; set; }
        public int StateId { get; set; }
        public bool Deleted { get; set; }

        public AspNetUsers Owner { get; set; }
        public ProjectState State { get; set; }
        public ICollection<ProjectMember> ProjectMember { get; set; }
        public ICollection<ProjectTechnology> ProjectTechnology { get; set; }
    }
}
