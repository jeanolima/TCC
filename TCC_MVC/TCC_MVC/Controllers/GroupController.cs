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
            IList<Curriculos> allResearchs = _curriculoBO.GetAll();
            model.ResearchsOut = allResearchs.OrderBy(x => x.Name).ToList();

            return View("New", model);
        }

        [HttpPost]
        public ActionResult New(GroupModel model)
        {

            return View();
        }

        public ActionResult Edit(int idGroup)
        {
            ViewBag.Title = "Editar Grupo";
            ViewBag.Action = "Edit";
            ViewBag.Controller = "Group";
            var group = _groupBO.GetGroupById(idGroup);
            GroupModel model = new GroupModel();
            IList<Curriculos> researchsIn = _groupBO.GetResearchsByGroup(idGroup);
            IList<Curriculos> allResearchs = _curriculoBO.GetAll();
            IList<Curriculos> researchsOut = allResearchs.Except(researchsIn, new CurriculoComparer()).ToList();
            
            model.Id = group.Id;
            model.Name = group.Name;
            model.Researchs = researchsIn.OrderBy(x => x.Name).ToList();
            model.ResearchsOut = researchsOut.OrderBy(x => x.Name).ToList();

            return View("Edit",model);
        }
        
        [HttpPost]
        public ActionResult Edit(GroupModel model)
        {

            return View();
        }

    }
}
