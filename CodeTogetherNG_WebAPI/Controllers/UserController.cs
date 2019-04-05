using CodeTogetherNG_WebAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet("{userId}")]
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

        [HttpGet]
        public JsonResult Users()
        {
            return new JsonResult(_context.AspNetUsers.Select(u => new
            {
                u.Id,
                u.UserName,
                Owner = u.Project.Select(m => new { m.OwnerId }).Count(),
                Member = u.ProjectMember.Select(m => new { m.MemberId }).Count(),
                Beginner = u.UserTechnologyLevel.Where(t =>  t.TechLevel==1).Count(),
                Advanced = u.UserTechnologyLevel.Where(t =>  t.TechLevel==2).Count(),
                Expert = u.UserTechnologyLevel.Where(t =>  t.TechLevel==3).Count()
            }));
        }
    }
}