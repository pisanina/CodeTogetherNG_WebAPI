using CodeTogetherNG_WebAPI.DTOs;
using CodeTogetherNG_WebAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeTogetherNG_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : CommonController
    {


        public ProjectsController(CodeTogetherNGContext context) : base(context)
        { }

        [HttpGet]
        public JsonResult Projects(string toSearch, int? projectState, bool? newMembers, List<int> techList)
        {
            var result = _context.Project.Where(p =>
            (p.Deleted==false)
        &&(projectState == null || p.StateId == projectState)
        && (newMembers == null || p.NewMembers == newMembers)
        && (techList == null || !techList.Except(p.ProjectTechnology.Select(t => t.TechnologyId)).Any())
        && (toSearch == null || p.Title.Contains(toSearch) || p.Description.Contains(toSearch))).
           Select(p => new
           {
               Id = p.Id,
               Title = p.Title,
               Description = p.Description,
               NewMembers = p.NewMembers,
               State = p.State.State,
               Technologies = p.ProjectTechnology.Select(t => t.Technology.TechName)
           });
            return new JsonResult(result);
        }

        [Route("{id}")]
        [HttpGet]
        public JsonResult ProjectDetails(int id)
        {
            return new JsonResult(_context.Project.Where(p => p.Id == id)
                                    .Select(p => new
                                    {
                                        Title = p.Title,
                                        Description = p.Description,
                                        Owner = new { Id = p.OwnerId, p.Owner.UserName },
                                        Member = p.ProjectMember.Where(a => a.AddMember == true).Select(m => new { m.Member.UserName, Id = m.MemberId }),
                                        CreationDate = p.CreationDate.ToString("dd/MM/yyyy"),
                                        NewMembers = p.NewMembers,
                                        Technologies = p.ProjectTechnology.Select(t => t.Technology.Id),
                                        State = p.State.Id
                                    }).Single());
        }

        [HttpPost, Authorize("jwt")]
        public async Task<IActionResult> PostProject([FromBody] AddProject addProject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = new Project();
            project.Title = addProject.Title;
            project.Description = addProject.Description;
            project.NewMembers = addProject.NewMembers;
            project.OwnerId = UserId;

            foreach (var item in addProject.Technologies)
            {
                ProjectTechnology tech = new ProjectTechnology
                {
                    TechnologyId = item
                };

                project.ProjectTechnology.Add(tech);
            }

            _context.Project.Add(project);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            // return CreatedAtAction(("GetProject", new { id = project.Id }, project);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [Route("Delete/{id}")]
        [HttpDelete, Authorize("jwt")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var OwnerName = _context.Project.Include(u => u.Owner).Single(i => i.Id == id).Owner.UserName;
           
            if (OwnerName.ToLower() == User.Identity.Name)
            {
                try
                {
                    var projectToDelete = _context.Project.Single(p => p.Id == id);
                    projectToDelete.Deleted = true;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
                return StatusCode((int)HttpStatusCode.OK);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }
        }

        [Route("Change")]
        [HttpPut, Authorize("jwt")]
        public async Task<IActionResult> ChangeProject([FromBody] ChangeProject changedProject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var isITowner = _context.Project.Include(u => u.Owner).Include(t => t.ProjectTechnology)
                .Single(i => i.Id == changedProject.ProjectId).Owner.UserName.ToLower() == User.Identity.Name.ToLower(); ;
            if (isITowner)
            {
                Project editedProject = _context.Project.Single(p => p.Id == changedProject.ProjectId);
                editedProject.Title = changedProject.Title;
                editedProject.Description = changedProject.Description;
                editedProject.NewMembers = changedProject.NewMembers;
                editedProject.StateId = changedProject.State;

                if (changedProject.Technologies.Count() != 0)
                {
                    editedProject.ProjectTechnology.Clear();
                }

                foreach (var item in changedProject.Technologies)
                {
                    ProjectTechnology tech = new ProjectTechnology
                    {
                        TechnologyId = item
                    };

                    editedProject.ProjectTechnology.Add(tech);
                }
                try
                {
                    await _context.SaveChangesAsync();
                    return StatusCode((int)HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

        }

        [Route("Request")]
        [HttpPost, Authorize("jwt")]
        public async Task<IActionResult> SendRequest([FromBody] RequestDto newRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectMember projectMember = new ProjectMember();
            projectMember.MemberId = UserId;
            projectMember.Message = newRequest.Message;
            projectMember.ProjectId = newRequest.ProjectId;

            _context.ProjectMember.Add(projectMember);

            try
            {
                await _context.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        [Route("RequestList/{projectId}")]
        [HttpGet, Authorize("jwt")]
        public IActionResult GetRequestByOwner(int projectId)
        {
            if (_context.Project.Single(p => p.Id == projectId).OwnerId == UserId)
            {

                return new JsonResult(_context.ProjectMember.Where(p => p.ProjectId == projectId && p.AddMember ==null)
                                        .Select(p => new
                                        {
                                            Id = p.Id,
                                            ProjectName = p.Project.Title,
                                            Date = p.MessageDate.ToString("dd/MM/yyyy"),
                                            MemberId = p.MemberId,
                                            MemberName = p.Member.UserName,
                                            Message = p.Message,
                                            Accepted = p.AddMember
                                        }));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }
        }


        [Route("Request/{projectId}")]
        [HttpGet, Authorize("jwt")]
        public IActionResult GetRequestByLoggedUser(int projectId)
        {
       
                var result = _context.ProjectMember.Where
                    (p => p.ProjectId == projectId  && p.MemberId ==UserId)
                                        .Select(p => new
                                        {
                                            Date = p.MessageDate,
                                            Accepted = p.AddMember
                                        }).OrderByDescending(p => p.Date).FirstOrDefault();

            if (result is null)
                return new JsonResult(new { Display=true, Message="" }); // first attempt
            if (result.Accepted == true)
                return new JsonResult (new { Display = false, Message = "" }); // already a member
            if (result.Accepted is null)
                return new JsonResult(new { Display = false, Message = "Your request is pending" });  // pending
            if (result.Accepted == false && result.Date.AddMonths(1) <= DateTime.Now)
                return new JsonResult (new { Display = true, Message = "" });  // rejected but long time ago
            return new JsonResult ( new
            { Display = false, Message = "Your unable to send a join request until " +
                    result.Date.AddMonths(1).ToString("dd/MM/yyyy")});  // rejected within 30 days

        }


        [Route("Request")]
        [HttpPut, Authorize("jwt")]
        public IActionResult AcceptRequest([FromBody] HandleRequestDto handleRequest)
        {            

            if (_context.Project.Single(p => p.Id == handleRequest.ProjectId).OwnerId == UserId)
            {
              var status = _context.ProjectMember.Single(p => 
                  p.ProjectId == handleRequest.ProjectId 
                  && p.MemberId ==  handleRequest.UserId 
                  && p.AddMember == null);
               status.AddMember = handleRequest.Accept;
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }
            try
            {
                _context.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }
    }
}
