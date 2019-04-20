using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.DTOs
{
    public class GetRequestDto
    {
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string Message { get; set; }
        public bool? Accepted { get; set; }
    }
}