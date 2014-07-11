using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.ArquivosBO;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class LineController : Controller
    {
        public GroupBO _groupBO = new GroupBO();
        public CurriculosBO _curriculoBO = new CurriculosBO();
        public ActionResult Index()
        {
            ViewBag.Title = "Index";
            IList<AreaModel> model = _groupBO.GetAreasBD();
            
            return View(model);
        }

        public ActionResult New()
        {
            ViewBag.Title = "Novo Grupo";
            ViewBag.Action = "New";
            ViewBag.Controller = "Line";
            LineModel model = new LineModel();
            IList<CurriculoModel> allResearchs = _curriculoBO.GetAll().Select(x => new CurriculoModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = false
            }).ToList();
            model.Researchs = allResearchs.OrderBy(x => x.Name).ToList();
            model.Areas = new List<AreaModel>();
            model.Areas.Add(new AreaModel { Id = 0, Name = "Selecione um grupo" });
            model.Areas = model.Areas.Concat(_groupBO.GetAreas()).ToList();

            return View("New", model);
        }

        [HttpPost]
        public ActionResult New(LineModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if (_groupBO.Save(model))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("New");
            }
            else
                return View(model);
        }

        public ActionResult Edit(int idLine)
        {
            ViewBag.Title = "Editar Grupo";
            ViewBag.Action = "Edit";
            ViewBag.Controller = "Line";
            var Line = _groupBO.GetLineById(idLine);
            LineModel model = new LineModel();
            var researchsIn = _groupBO.GetResearchsByLine(idLine).Select(x => new CurriculoModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = true
            }).OrderBy(x => x.Name).ToList();

            var researchsOut = _curriculoBO.GetAll().Select(x => new CurriculoModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = false
            }).Except(researchsIn, new CurriculoComparer()).OrderBy(x => x.Name).ToList();

            var allResearchs = researchsIn.Concat(researchsOut).ToList();

            model.Id = Line.Id;
            model.Name = Line.Name;
            model.Researchs = allResearchs;
            model.Areas = new List<AreaModel>();
            model.Areas.Add(new AreaModel { Id = 0, Name = "Selecione um grupo" });
            model.Areas = model.Areas.Concat(_groupBO.GetAreas()).ToList();
            var areaSelected = _groupBO.GetAreaById(Line.AreaId);
            model.AreaSelected = areaSelected.Id;

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(LineModel model)
        {

            if (!string.IsNullOrEmpty(model.Name) && model.AreaSelected != 0)
            {
                if (_groupBO.Save(model))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Edit", model.Id);
            }
            else
                return View(model);
        }

    }
}
