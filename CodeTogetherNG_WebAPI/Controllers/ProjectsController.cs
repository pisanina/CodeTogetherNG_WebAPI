﻿using System;
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
    public class ProjectsController : ControllerBase
    {
        private readonly CodeTogetherNGContext _context;

        public ProjectsController(CodeTogetherNGContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public JsonResult Projects(string toSearch, int? projectState, bool? newMembers, List<int> techList)
        {
            return new JsonResult(_context.Project.Where(p =>
        (projectState == null || p.StateId == projectState)
        && (newMembers == null || p.NewMembers == newMembers)
        && (techList == null || !techList.Except(p.ProjectTechnology.Select(t => t.TechnologyId)).Any())
        && (toSearch == null || p.Title.Contains(toSearch) || p.Description.Contains(toSearch))).
           Select(p => new
           {
               Id = p.Id,
               Title = p.Title,
               Description = p.Description,
               NewMembers = p.NewMembers,
               Technologies = p.ProjectTechnology.Select(t => t.Technology.TechName)
           }));
        }
    }
}




//[HttpPut("{id}")]
//public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
//{
//    if (!ModelState.IsValid)
//    {
//        return BadRequest(ModelState);
//    }

//    if (id != project.Id)
//    {
//        return BadRequest();
//    }

//    _context.Entry(project).State = EntityState.Modified;

//    try
//    {
//        await _context.SaveChangesAsync();
//    }
//    catch (DbUpdateConcurrencyException)
//    {
//        if (!ProjectExists(id))
//        {
//            return NotFound();
//        }
//        else
//        {
//            throw;
//        }
//    }

//    return NoContent();
//}

//// POST: api/Projects
//[HttpPost]
//public async Task<IActionResult> PostProject([FromBody] Project project)
//{
//    if (!ModelState.IsValid)
//    {
//        return BadRequest(ModelState);
//    }

//    _context.Project.Add(project);
//    await _context.SaveChangesAsync();

//    return CreatedAtAction("GetProject", new { id = project.Id }, project);
//}

//// DELETE: api/Projects/5
//[HttpDelete("{id}")]
//public async Task<IActionResult> DeleteProject([FromRoute] int id)
//{
//    if (!ModelState.IsValid)
//    {
//        return BadRequest(ModelState);
//    }

//    var project = await _context.Project.FindAsync(id);
//    if (project == null)
//    {
//        return NotFound();
//    }

//    _context.Project.Remove(project);
//    await _context.SaveChangesAsync();

//    return Ok(project);
//}

//private bool ProjectExists(int id)
//{
//    return _context.Project.Any(e => e.Id == id);
//}
