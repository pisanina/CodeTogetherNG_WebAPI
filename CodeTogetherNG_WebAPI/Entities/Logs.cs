using System;
using System.Collections.Generic;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class Logs
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string ErrorMessage { get; set; }
    }
}
