using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.DTOs
{
    public class HandleRequestDto
    {
        public int ProjectId  { get; set; }
        public string UserId { get; set; }
        public bool Accept    { get; set; }
    }
}