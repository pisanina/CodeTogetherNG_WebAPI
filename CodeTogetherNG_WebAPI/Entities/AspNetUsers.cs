using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class AspNetUsers : IdentityUser
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            Project = new HashSet<Project>();
            ProjectMember = new HashSet<ProjectMember>();
            UserTechnologyLevel = new HashSet<UserTechnologyLevel>();
        }

        public ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public ICollection<Project> Project { get; set; }
        public ICollection<ProjectMember> ProjectMember { get; set; }
        public ICollection<UserTechnologyLevel> UserTechnologyLevel { get; set; }
    }
}
