using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TCC_MVC.ArquivosBO;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class ApplicationController : Controller
    {
        public CurriculosBO _curriculosBO = new CurriculosBO();
        public GroupBO _groupBO = new GroupBO();

        public ActionResult Index()
        {
            var model = new SearchModel();
            model.Groups = new List<GroupModel>();
            model.Groups.Add(new GroupModel { Id = 0, Name = "Selecione um grupo" });
            model.Groups = model.Groups.Concat(_groupBO.GetAll().Select(x => new GroupModel
            {
                Id = x.Id,
                Name = x.Name
            })).ToList();

            model.Lines = new List<LineModel>();
            model.Lines.Add(new LineModel { Id = 0, Name = "Selecione a linha de pesquisa" });
            model.Lines = model.Lines.Concat(_groupBO.GetAllLines()).ToList();
            
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            string keytype = "";
            
            keytype = model.KeyType;
            switch (model.KeyType)
            {
                case "author": {
                    if (!string.IsNullOrEmpty(model.Keyword))
                    {
                        model = _curriculosBO.GetResearchsByAuthor(model, model.Keyword); 
                    }
                    else
                    {
                        model = _curriculosBO.GetAllArticles();
                    }
                    break; 
                }
                case "search": { model = _curriculosBO.GetResearchBySearch(model, model.LineSelected); break; }
                case "group": { model = _curriculosBO.GetResearchByGroup(model, model.GroupSelected); break; }
                default: { model = _curriculosBO.GetResearchsByAuthor(model, model.Keyword); break; };
            }
            
            
            switch (model.OrderByType)
            {
                case "year": { model = _curriculosBO.OrderByYear(model); break; }
                case "triennium": { model = _curriculosBO.OrderByTriennium(model); break; }
                //case "total": { model = _curriculosBO.OrderByYear(model); break; }
                case "evolution": { 
                    model.EvolutionGrafic = _curriculosBO.CountByEvolution(model);
                    model.articles= _curriculosBO.OrderByEvolution(model);
                    break;
                }
                default: break;
            }

            switch (model.GroupByType)
            {
                case "r/c": { model = _curriculosBO.OrderByCompleteType(model); break; }
                case "p/e": { model = _curriculosBO.OrderByPeriodicType(model); break; }
                //case "periodico": { model = _curriculosBO.OrderByType(model); break; }
                default: break;
            }

            if (model.Qualis != null)
            {
                if (model.Qualis.specificCheck)
                {
                    model.Qualis = _curriculosBO.CountAllSpecificQualis(model);
                    model.showQualis = true;
                }
                else
                {
                    if (model.Qualis.a1Check) { _curriculosBO.CountSpecificQualis(model, "a1"); model.showQualis = true; }
                    if (model.Qualis.a2Check) { _curriculosBO.CountSpecificQualis(model, "a2"); model.showQualis = true; }
                    if (model.Qualis.b1Check) { _curriculosBO.CountSpecificQualis(model, "b1"); model.showQualis = true; }
                    if (model.Qualis.b2Check) { _curriculosBO.CountSpecificQualis(model, "b2"); model.showQualis = true; }
                    if (model.Qualis.b3Check) { _curriculosBO.CountSpecificQualis(model, "b3"); model.showQualis = true; }
                    if (model.Qualis.b4Check) { _curriculosBO.CountSpecificQualis(model, "b4"); model.showQualis = true; }
                    if (model.Qualis.b5Check) { _curriculosBO.CountSpecificQualis(model, "b5"); model.showQualis = true; }
                    if (model.Qualis.cCheck) { _curriculosBO.CountSpecificQualis(model, "c"); model.showQualis = true; }
                }
                if (model.Qualis.withCheck)
                {
                    model.Qualis = _curriculosBO.CountAllQualis(model);
                    model.showQualis = true;
                }
                if (model.Qualis.withoutCheck)
                {
                    model.Qualis = _curriculosBO.CountAllWithoutQualis(model);
                    model.showQualis = true;
                }
            }

            model.Groups = new List<GroupModel>();
            model.Groups.Add(new GroupModel { Id = 0, Name = "Selecione um grupo" });
            model.Groups = model.Groups.Concat(_groupBO.GetAll().Select(x => new GroupModel
            {
                Id = x.Id,
                Name = x.Name
            })).ToList();

            model.Lines = new List<LineModel>();
            model.Lines.Add(new LineModel { Id = 0, Name = "Selecione a linha de pesquisa" });
            model.Lines = model.Lines.Concat(_groupBO.GetAllLines()).ToList();
            model.KeyType = keytype;
            return View("Index", model);
        }

        public ActionResult Research(int id)
        {
            var model = new SearchModel();
            model.Groups = new List<GroupModel>();
            model.Groups.Add(new GroupModel { Id = 0, Name = "Selecione um grupo" });
            model.Groups = model.Groups.Concat(_groupBO.GetAll().Select(x => new GroupModel
            {
                Id = x.Id,
                Name = x.Name
            })).ToList();
            model = _curriculosBO.GetModelByResearchId(model, id);

            model.Lines = new List<LineModel>();
            model.Lines.Add(new LineModel { Id = 0, Name = "Selecione a linha de pesquisa" });
            model.Lines = model.Lines.Concat(_groupBO.GetAllLines()).ToList();

            return View("Index", model);
        }
    }
}
