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
    public class UserController : ControllerBase
    {
        private readonly CodeTogetherNGContext _context;

        public UserController(CodeTogetherNGContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public JsonResult Profile(string userId)
        {

            var userSkills = _context.UserTechnologyLevel.Where(u => u.UserId == userId)
                .Select(t => new { t.Technology.TechName, t.TechLevel });

            var userOwner = _context.Project.Where(o => o.OwnerId == userId)
                .Select(u => new {u.Id, u.Title });
            var userMember = _context.ProjectMember.Where(m => (m.MemberId == userId)
                                && (m.AddMember == true)).Select(u => new {u.Project.Id, u.Project.Title });


            return new JsonResult(new { userSkills, userOwner, userMember });

        }
    }
}