using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCC_MVC.ArquivosBO;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class GroupController : Controller
    {
        public GroupBO _groupBO = new GroupBO();
        public CurriculosBO _curriculoBO = new CurriculosBO();
        public ActionResult Index()
        {
            ViewBag.Title = "Index";
            IList<GroupModel> model = _groupBO.GetAllGroups();

            return View(model);
        }

        public ActionResult New()
        {
            ViewBag.Title = "Novo Grupo";
            ViewBag.Action= "New";
            ViewBag.Controller = "Group";
            GroupModel model = new GroupModel();
            IList<CurriculoModel> allResearchs = _curriculoBO.GetAll().Select(x => new CurriculoModel { 
                Id = x.Id,
                Name = x.Name,
                IsSelected = false
            }).ToList();
            model.Researchs = allResearchs.OrderBy(x => x.Name).ToList();

            return View("New", model);
        }

        [HttpPost]
        public ActionResult New(GroupModel model)
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

        public ActionResult Edit(int idGroup)
        {
            ViewBag.Title = "Editar Grupo";
            ViewBag.Action = "Edit";
            ViewBag.Controller = "Group";
            var group = _groupBO.GetGroupById(idGroup);
            GroupModel model = new GroupModel();
            var researchsIn = _groupBO.GetResearchsByGroup(idGroup).Select(x => new CurriculoModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = true
            }).OrderBy(x=>x.Name).ToList();

            var researchsOut = _curriculoBO.GetAll().Select(x => new CurriculoModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = false
            }).Except(researchsIn, new CurriculoComparer()).OrderBy(x => x.Name).ToList();

            var allResearchs = researchsIn.Concat(researchsOut).ToList();
            
            model.Id = group.Id;
            model.Name = group.Name;
            model.Researchs = allResearchs;

            return View("Edit",model);
        }
        
        [HttpPost]
        public ActionResult Edit(GroupModel model)
        {

            if (!string.IsNullOrEmpty(model.Name))
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
