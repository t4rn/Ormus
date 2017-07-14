using AutoMapper;
using System;
using System.Web.Mvc;

namespace Ormus.Web.Controllers.Web
{
    public abstract class BaseController : Controller
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected ActionResult Exception(Exception ex)
        {
            return View(viewName: "Error", model: ex.ToString());
        }

        // GET: Base
        protected ActionResult Error(string error)
        {
            return View(viewName:"Error", model: error);
        }
    }
}