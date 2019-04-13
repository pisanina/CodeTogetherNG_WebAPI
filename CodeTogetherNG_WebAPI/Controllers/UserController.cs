using CodeTogetherNG_WebAPI.DTOs;
using CodeTogetherNG_WebAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CommonController
    {
        private UserManager<AspNetUsers> _userManager;
        private SignInManager<AspNetUsers> _signInManager;
        private IConfiguration _configuration;

        public UserController(CodeTogetherNGContext context, UserManager<AspNetUsers> userManager
        , SignInManager<AspNetUsers> signInManager, IConfiguration configuration) :base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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

            var userITRole = _context.UserITRole.Where(r => r.UserId == userId)
                .Select(n => new { n.RoleId, n.Role.Role });

            return new JsonResult(new { userSkills, userOwner, userMember, userITRole });
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
                Beginner = u.UserTechnologyLevel.Where(t => t.TechLevel == 1).Count(),
                Advanced = u.UserTechnologyLevel.Where(t => t.TechLevel == 2).Count(),
                Expert = u.UserTechnologyLevel.Where(t => t.TechLevel == 3).Count(),
                ITRole = u.UserITRole.Select(r => new { r.Role }).Count()
            }));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = new AspNetUsers {UserName = userDto.Username, Email = userDto.Username };
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                    return StatusCode((int)HttpStatusCode.Created);
                else
                    return StatusCode((int)HttpStatusCode.BadRequest, result.Errors);
            }
            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result =
                        await _signInManager.PasswordSignInAsync(userDto.Username, userDto.Password, true, true);
                    if (result.Succeeded)
                    {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtSecretKey"]));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        var tokeOptions = new JwtSecurityToken(
                            claims: new[]
                            {
                                new Claim(ClaimTypes.Name, userDto.Username)
                            },
                            expires: DateTime.Now.AddMinutes(5),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                        return Ok(new { Token = tokenString });
                    }
                }
                catch (Exception e)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [Route("ITRole/{id}")]
        [HttpDelete, Authorize("jwt")]
        public async Task<IActionResult> DeleteITRole(int id)
        {
            try
            {
                var roleToDelete = _context.UserITRole.Single(r => r.RoleId == id && r.UserId == UserId);
                _context.UserITRole.Remove(roleToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return StatusCode((int)HttpStatusCode.OK);
        }

        [Route("ITRole")]
        [HttpPost, Authorize("jwt")]
        public async Task<IActionResult> AddITRole([FromBody] int roleId)
        {
            try
            {
                var  role = new UserITRole
                {
                    UserId = this.UserId,
                    RoleId = roleId
                };

                _context.UserITRole.Add(role);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return StatusCode((int)HttpStatusCode.Created);
        }


        [Route("Tech")]
        [HttpPost, Authorize("jwt")]
        public async Task<IActionResult> AddTech([FromBody] TechDto techDto)
        {
            try
            {
                var  newTech = new UserTechnologyLevel
                {
                    UserId = this.UserId,
                    TechnologyId = techDto.TechnologyId,
                    TechLevel = techDto.TechLevel
                };

                _context.UserTechnologyLevel.Add(newTech);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        [Route("Tech/{id}")]
        [HttpDelete, Authorize("jwt")]
        public async Task<IActionResult> DeleteTech(int id)
        {
            try
            {
                var techToDelete = _context.UserTechnologyLevel.Single(r => r.TechnologyId == id && r.UserId == UserId);
                _context.UserTechnologyLevel.Remove(techToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}