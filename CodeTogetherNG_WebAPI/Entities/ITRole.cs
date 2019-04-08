using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.Entities
{
    public class ITRole
    {
        public ITRole()
        {
            UserITRole = new HashSet<UserITRole>();
        }

        public int Id { get; set; }
        public string Role { get; set; }

        public ICollection<UserITRole> UserITRole { get; set; }
    }
}