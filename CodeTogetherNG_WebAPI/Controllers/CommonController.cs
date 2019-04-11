using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeTogetherNG_WebAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogetherNG_WebAPI.Controllers
{
   
    public abstract class CommonController : ControllerBase
    {

        protected readonly CodeTogetherNGContext _context;
        private string _userId;

        public CommonController(CodeTogetherNGContext context)
        {
            _context = context;
            
        }

        protected string UserId {
            get
            {
                if (_userId is null)
                {
                    _userId = _context.AspNetUsers.Single(u => u.UserName == User.Identity.Name).Id;
                }
                return _userId;

            }
        }
    }
}