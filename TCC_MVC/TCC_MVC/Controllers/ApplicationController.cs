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

        public ActionResult Index()
        {
            var model = new SearchModel();

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            switch (model.KeyType)
            {
                case "author": { model = _curriculosBO.GetResearchsByAuthor(model, model.Keyword); break; }
                case "search": { model = _curriculosBO.GetResearchBySearch(model, model.Keyword); break; }
                case "group": { model = _curriculosBO.GetResearchByGroup(model, model.Keyword); break; }
                case "clique": { /* model = GetResearchByClique(model, model.Keyword);*/ break; }
                default: break;
            }

            switch (model.OrderByType)
            {
                case "year": { model = _curriculosBO.OrderByYear(model); break; }
                case "triennum": { model = _curriculosBO.OrderByYear(model); break; }
                case "total": { model = _curriculosBO.OrderByYear(model); break; }
                case "evolution": { 
                    model.EvolutionGrafic = _curriculosBO.CountByEvolution(model, model.EvolutionType, model.EvolutionGap);
                    model.articles= _curriculosBO.OrderByEvolution(model, model.EvolutionType, model.EvolutionGap);
                    model.EvolutionType = (model.EvolutionType.Equals("year") ? "anos" : "trienos");
                    break;
                }
                default: break;
            }

            switch (model.GroupByType)
            {
                case "completo": { model = _curriculosBO.GroupByCompleteType(model); break; }
                case "resumo": { model = _curriculosBO.GroupByResumeType(model); break; }
                case "periodico": { /*model = OrderByYear(model); */break; }
                default: break;
            }

            return View("Index", model);
        }
    }
}
