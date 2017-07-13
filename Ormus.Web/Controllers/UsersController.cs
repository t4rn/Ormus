using Ormus.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ormus.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public ActionResult List()
        {
            var users = _userRepo.GetAll();

            return View(users);
        }
    }
}