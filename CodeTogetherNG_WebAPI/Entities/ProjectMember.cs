using System;
using System.Collections.Generic;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class ProjectMember
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string MemberId { get; set; }
        public DateTimeOffset MessageDate { get; set; }
        public string Message { get; set; }
        public bool? AddMember { get; set; }

        public AspNetUsers Member { get; set; }
        public Project Project { get; set; }
    }
}
