using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeTogetherNG_WebAPI.Entities;

namespace CodeTogetherNG_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechListController : ControllerBase
    {

        private readonly CodeTogetherNGContext _context;

        public TechListController(CodeTogetherNGContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult TechnologyList()
        {
            return new JsonResult(_context.Technology.Select(t => new { t.Id, t.TechName }));
        }
    }
}