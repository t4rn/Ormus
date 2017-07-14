using AutoMapper;
using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using Ormus.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Ormus.Web.Controllers.Web
{
    public class UsersController : BaseController
    {
        private readonly IUserRepository _repo;
        private readonly IUserRoleRepository _rolesRepo;

        public UsersController(IMapper mapper,
            IUserRepository repo,
            IUserRoleRepository rolesRepo) : base(mapper)
        {
            _repo = repo;
            _rolesRepo = rolesRepo;
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var users = _repo.GetAll().OrderBy(x => x.Id);
                IEnumerable<UserModel> usersModel = _mapper.Map<IEnumerable<UserModel>>(users);

                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];

                return View(usersModel);
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            UserModel model = new UserModel();
            model.UserRolesCollection = PrepareUserRolesCollection();

            return View(model);
        }

        private IEnumerable<SelectListItem> PrepareUserRolesCollection()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            IEnumerable<UserRole> userRoles = _rolesRepo.GetAll().Where(x => x.Ghost == false);

            var userRolesModel = _mapper.Map<IEnumerable<UserRoleModel>>(userRoles);

            foreach (var userRole in userRolesModel)
            {
                SelectListItem item = new SelectListItem() { Text = userRole.Code, Value = userRole.Id.ToString() };
                selectList.Add(item);
            }

            return selectList;
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _mapper.Map<User>(model);
                    user.CreatedDate = DateTime.Now;

                    _repo.Add(user);

                    if (user.Id <= 0)
                    {
                        ViewBag.Error = $"Failed to created user of Login = '{model.Login}'!";
                        return View(model);
                    }
                    else
                    {

                        TempData["Message"] = $"User with ID = '{user.Id}' created!";
                        return RedirectToAction("Index");
                    }
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

        [HttpGet]
        public ActionResult Details(int id)
        {
            User user = _repo.Get(id);

            if (user != null)
            {
                UserModel model = _mapper.Map<UserModel>(user);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            User user = _repo.Get(id);

            if (user != null)
            {
                UserModel model = _mapper.Map<UserModel>(user);
                model.UserRolesCollection = PrepareUserRolesCollection();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                User user = _repo.Get(model.Id);
                if (user == null)
                {
                    ViewBag.Error = $"User of ID = '{model.Id}' not found!";
                    return View(model);
                }

                _mapper.Map(model, user);
                user.UpdatedDate = DateTime.Now;

                int rowsAffected = _repo.Update(user);

                if (rowsAffected <= 0)
                {
                    ViewBag.Error = $"Update failed!";
                    return View(model);
                }
                else
                {
                    TempData["Message"] = $"User of ID = '{model.Id}' edited successfully!";

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
            User user = _repo.Get(id);

            if (user != null)
            {
                UserModel model = _mapper.Map<UserModel>(user);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, UserModel model)
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
                    TempData["Message"] = $"User of ID = '{id}' deleted!";

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