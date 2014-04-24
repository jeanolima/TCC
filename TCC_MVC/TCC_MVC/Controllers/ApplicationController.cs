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
            if (!string.IsNullOrEmpty(model.KeyType))
            {
                switch (model.KeyType)
                {
                    case "author": { model = _curriculosBO.GetResearchsByAuthor(model, model.Keyword); break; }
                    case "search": { model = _curriculosBO.GetResearchBySearch(model, model.Keyword); break; }
                    case "group": { model = _curriculosBO.GetResearchByGroup(model, model.Keyword); break; }
                    case "clique": { /* model = GetResearchByClique(model, model.Keyword);*/ break; }
                    default: { model = _curriculosBO.GetResearchsByAuthor(model, model.Keyword); break; };
                }
                model.Total = model.articles.Count;
            }
            else
            {
                model = _curriculosBO.GetAllArticles();
            }

            switch (model.OrderByType)
            {
                case "year": { model = _curriculosBO.OrderByYear(model); break; }
                case "triennum": { model = _curriculosBO.OrderByYear(model); break; }
                case "total": { model = _curriculosBO.OrderByYear(model); break; }
                case "evolution": { 
                    model.EvolutionGrafic = _curriculosBO.CountByEvolution(model);
                    model.articles= _curriculosBO.OrderByEvolution(model);
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

            if (model.Qualis.specificCheck != null && model.Qualis.specificCheck)
            {
                model.Qualis = _curriculosBO.CountAllSpecificQualis(model);
            }
            else
            {
                model.Qualis = (model.Qualis.a1Check) ? _curriculosBO.CountSpecificQualis(model, "a1") : model.Qualis;
                model.Qualis = (model.Qualis.a2Check) ? _curriculosBO.CountSpecificQualis(model, "a2") : model.Qualis;
                model.Qualis = (model.Qualis.b1Check) ? _curriculosBO.CountSpecificQualis(model, "b1") : model.Qualis;
                model.Qualis = (model.Qualis.b2Check) ? _curriculosBO.CountSpecificQualis(model, "b2") : model.Qualis;
                model.Qualis = (model.Qualis.b3Check) ? _curriculosBO.CountSpecificQualis(model, "b3") : model.Qualis;
                model.Qualis = (model.Qualis.b4Check) ? _curriculosBO.CountSpecificQualis(model, "b4") : model.Qualis;
                model.Qualis = (model.Qualis.b5Check) ? _curriculosBO.CountSpecificQualis(model, "b5") : model.Qualis;
                model.Qualis = (model.Qualis.cCheck) ? _curriculosBO.CountSpecificQualis(model, "c") : model.Qualis;
            }
            if(model.Qualis.withCheck != null && model.Qualis.withCheck)
                model.Qualis = _curriculosBO.CountAllQualis(model);
            if (model.Qualis.withoutCheck != null && model.Qualis.withoutCheck)
                model.Qualis = _curriculosBO.CountAllWithoutQualis(model);

            return View("Index", model);
        }
    }
}
