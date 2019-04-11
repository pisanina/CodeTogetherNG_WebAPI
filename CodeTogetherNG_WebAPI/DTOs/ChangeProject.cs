using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.DTOs
{
    public class ChangeProject
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(1000)]
        public string Description { get; set; }

        public bool NewMembers { get; set; }
        public int State { get; set; }
        public List<int> Technologies { get; set; }
        public int ProjectId { get; set; }

    }
}
