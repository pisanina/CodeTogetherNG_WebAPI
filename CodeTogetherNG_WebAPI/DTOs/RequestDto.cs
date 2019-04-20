using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.DTOs
{
    public class RequestDto
    {
        public int ProjectId { get; set; }
        public string Message { get; set; }
    }
}