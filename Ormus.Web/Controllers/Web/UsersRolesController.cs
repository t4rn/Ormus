using AutoMapper;
using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using Ormus.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ormus.Web.Controllers.Web
{
    public class UsersRolesController : BaseController
    {
        private readonly IUserRoleRepository _repo;

        public UsersRolesController(IUserRoleRepository repo, IMapper mapper) : base(mapper)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            IEnumerable<UserRole> userRoles = _repo.GetAll();

            IEnumerable<UserRoleModel> model = _mapper.Map<IEnumerable<UserRoleModel>>(userRoles);

            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            UserRoleModel model = new UserRoleModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserRoleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserRole userRole = _mapper.Map<UserRole>(model);
                    userRole.CreatedDate = DateTime.Now;

                    _repo.Add(userRole);

                    if (userRole.Id <= 0)
                    {
                        TempData["Error"] = "Insert failed!";
                    }
                    else
                    {
                        TempData["Message"] = $"User Role with Code = '{model.Code}' added successfully!";
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        public ActionResult Details(int id)
        {
            UserRole userRole = _repo.Get(id);

            if (userRole != null)
            {
                UserRoleModel model = _mapper.Map<UserRoleModel>(userRole);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            UserRole userRole = _repo.Get(id);

            if (userRole != null)
            {
                UserRoleModel model = _mapper.Map<UserRoleModel>(userRole);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(UserRoleModel model)
        {
            try
            {
                UserRole userRole = _repo.Get(model.Id);
                if (userRole == null)
                {
                    ViewBag.Error = $"User Role of ID = '{model.Id}' not found!";
                    return View(model);
                }

                _mapper.Map(model, userRole);

                int rowsAffected = _repo.Update(userRole);

                if (rowsAffected <= 0)
                {
                    ViewBag.Error = $"Update failed!";
                    return View(model);
                }
                else
                {
                    TempData["Message"] = $"User Role of ID = '{model.Id}' edited successfully!";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            UserRole userRole = _repo.Get(id);

            if (userRole != null)
            {
                UserRoleModel model = _mapper.Map<UserRoleModel>(userRole);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, UserRoleModel model)
        {
            try
            {
                int rowsAffected = _repo.Delete(id);

                if (rowsAffected <= 0)
                {
                    ViewBag.Error = $"Delete for ID = '{id}' failed!";
                    return View(model);
                }
                else
                {
                    TempData["Message"] = $"User Role of ID = '{id}' deleted!";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }
    }
}
