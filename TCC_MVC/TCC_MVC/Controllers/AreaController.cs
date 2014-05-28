using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.ArquivosBO;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class AreaController : Controller
    {
        public GroupBO _areaBO = new GroupBO();

        public ActionResult Index()
        {
            var model = _areaBO.GetAreas();

            return View(model);
        }

        public ActionResult New()
        {
            ViewBag.Title = "Nova Linha de Pesquisa";
            ViewBag.Action = "New";
            ViewBag.Controller = "Area";
            var model = new AreaModel();

            return View("New", model);
        }

        [HttpPost]
        public ActionResult New(AreaModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if (_areaBO.Save(model))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("New");
            }
            else
                return View(model);
        }

        public ActionResult Edit(int idArea)
        {
            ViewBag.Title = "Editar Linha";
            ViewBag.Action = "Edit";
            ViewBag.Controller = "Area";
            var Line = _areaBO.GetAreaById(idArea);
            AreaModel model = new AreaModel();
            
            model.Id = Line.Id;
            model.Name = Line.Name;
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(AreaModel model)
        {

            if (!string.IsNullOrEmpty(model.Name))
            {
                if (_areaBO.Save(model))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Edit", model.Id);
            }
            else
                return View(model);
        }
    }
}
