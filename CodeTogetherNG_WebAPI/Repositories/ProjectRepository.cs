using CodeTogetherNG_WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.Repositories
{
    public class ProjectRepository
    {

        private readonly CodeTogetherNGContext _context;

        public ProjectRepository(CodeTogetherNGContext context)
        {
            _context = context;
        }
    }
}
