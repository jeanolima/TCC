using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class ApplicationController : Controller
    {
        //
        // GET: /XML/

        public ActionResult Index()
        {
            var model = new SearchModel();

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            switch(model.KeyType)
            {
                case "author": { model = GetResearchsByAuthor(model, model.Keyword); break; }
                case "search": {model= GetResearchBySearch(model, model.Keyword); break; }
                case "group": { model= GetResearchByGroup(model, model.Keyword); break; }
                case "clique": {/* model = GetResearchByClique(model, model.Keyword);*/ break; }
                default: break;
            }

            switch (model.OrderByType)
            {
                case "year":{ model = OrderByYear(model); break; }
                case "triennum": { model = OrderByYear(model); break; }
                case "total": { model = OrderByYear(model); break; }
                case "evolution": { /*model = OrderByYear(model); */break; }
                default: break;
            }

            switch (model.GroupByType)
            {
                case "completo": { model = GroupByCompleteType(model); break; }
                case "resumo": { model = GroupByResumeType(model); break; }
                case "periodico": { /*model = OrderByYear(model); */break; }
                default: break;
            }

            return View("Index", model);
        }

        private int CountAllQualis (SearchModel model)
        {
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//INFORMACOES-ADICIONAIS";
            foreach (var author in model.curriculos)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data);

                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    var info = node.Attributes["DESCRICAO-INFORMACOES-ADICIONAIS"].Value;
                    if(info.Contains("Qualis") &&
                        !info.Contains("Nada consta") &&
                        !info.Contains("Não está classificado"))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        private int CountAllWithoutQualis(SearchModel model)
        {
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//INFORMACOES-ADICIONAIS";
            foreach (var author in model.curriculos)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data);

                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    var info = node.Attributes["DESCRICAO-INFORMACOES-ADICIONAIS"].Value;
                    if (!info.Contains("Qualis") ||
                        info.Contains("Nada consta") ||
                        info.Contains("Não está classificado"))
                    {
                        total++;
                    }
                }
            }

            return total;
        }
        
        private int CountSpecificQualis(SearchModel model, string qualis)
        {
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//INFORMACOES-ADICIONAIS";
            foreach (var author in model.curriculos)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data);

                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    var info = node.Attributes["DESCRICAO-INFORMACOES-ADICIONAIS"].Value;
                    if (info.Contains(qualis) &&
                        !info.Contains("Nada consta") &&
                        !info.Contains("Não está classificado"))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        //não esta pronta
        private SearchModel CountAllSpecificQualis(SearchModel model)
        {
            int total = 0;
            var listaQualis = new List<string>() { 
                "A1", "1A", "a1", "1a",
                "A2", "2A", "a2", "2a",
                "B1", "1B", "b1", "1b",
                "B2", "2B", "b2", "2b",
                "B3", "3B", "b3", "3b",
                "B4", "4B", "b4", "4b",
                "B5", "5B", "b5", "5b",
                "C"
            };
            
            foreach (var author in model.curriculos)
            {
                foreach (var qualis in listaQualis)
                {
                    string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//INFORMACOES-ADICIONAIS[contains(@DESCRICAO-INFORMACOES-ADICIONAIS, '" + qualis + "') and not(contains(@DESCRICAO-INFORMACOES-ADICIONAIS,'Nada consta') or contains(@DESCRICAO-INFORMACOES-ADICIONAIS,'Não está classificado')) ]";
                
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(author.Data);
                    total = xml.SelectNodes(xpathArticle).Count;

                    var num = Regex.Match(qualis, @"\d+").Value;
                    var letter = Regex.Replace(qualis, @"[\d-]", string.Empty);
                    var qualisFormat = letter + num;
                    switch (qualisFormat.ToLower())
                    {
                        case "a1": { model.TotalA1 = model.TotalA1 + total; break; }
                        case "a2": { model.TotalA2 = model.TotalA2 + total; break; }
                        case "b1": { model.TotalB1 = model.TotalB1 + total; break; }
                        case "b2": { model.TotalB2 = model.TotalB2 + total; break; }
                        case "b3": { model.TotalB3 = model.TotalB3 + total; break; }
                        case "b4": { model.TotalB4 = model.TotalB4 + total; break; }
                        case "b5": { model.TotalB5 = model.TotalB5 + total; break; }
                        case "c": { model.TotalC = model.TotalC + 1; break; }
                        default: { break; }
                    }
                }
            }
            return model;
        }

        private SearchModel GroupByCompleteType(SearchModel model)
        {
            model.articles = model.articles.Where(x => x.Nature.ToLower().Equals("completo")).ToList();
            return model;
        }

        private SearchModel GroupByResumeType(SearchModel model)
        {
            model.articles = model.articles.Where(x => x.Nature.ToLower().Equals("resumo")).ToList();
            return model;
        }

        private SearchModel OrderByTriennium(SearchModel model)
        {
            var yearNow = DateTime.Now.Year;

            model.articles = model.articles.Where(x => (x.Year > yearNow - 3 ? true : false)).OrderBy(x => x.Year).ToList();

            var years = model.articles.Select(x => x.Year).Distinct().ToList();
            model.Years = new List<YearArticlesModel>();

            foreach (var year in years)
            {
                var totalOfYear = model.articles.Where(x => x.Year.Equals(year)).Count();

                model.Years.Add(new YearArticlesModel
                {
                    TotalArticles = totalOfYear,
                    Year = year
                });
            }

            return model;
        }

        private SearchModel OrderByYear (SearchModel model)
        {
            model.articles = model.articles.OrderBy(x => x.Year).ToList();

            var years = model.articles.Select(x => x.Year).Distinct().ToList();
            model.Years = new List<YearArticlesModel>();
            
            foreach (var year in years)
            {
                var totalOfYear = model.articles.Where(x => x.Year.Equals(year)).Count();

                model.Years.Add(new YearArticlesModel{
                    TotalArticles = totalOfYear,
                    Year = year
                });
            }

            return model;
        }

        private SearchModel GetAllArticles()
        {
            var model = new SearchModel();
            var researches = GetAll();
            model.curriculos =researches;
            model = GetArticlesFromXML(model, researches, true);

            return model;
        }

        private SearchModel GetResearchsByAuthor(SearchModel model, string name)
        {
            
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS[contains(@NOME-COMPLETO,'" + name + "') or @NOME-COMPLETO= '" + name + "']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model = GetArticlesFromXML(model, research);

                return model;
            }
        }


        private SearchModel GetResearchBySearch(SearchModel model, string title)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//ATUACOES-PROFISSIONAIS//ATUACAO-PROFISSIONAL//LINHA-DE-PESQUISA[contains(@TITULO-DA-LINHA-DE-PESQUISA, '" + title + "') or @TITULO-DA-LINHA-DE-PESQUISA = '"+title+"']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model = GetArticlesFromXML(model, research);

                return model;
            }
        }

        private SearchModel GetResearchByGroup(SearchModel model, string projectName)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//ATUACOES-PROFISSIONAIS//ATUACAO-PROFISSIONAL//PROJETO-DE-PESQUISA[contains(@NOME-DO-PROJETO, '" + projectName + "') or @NOME-DO-PROJETO = '" + projectName + "']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => entry.Id == 13 && XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model = GetArticlesFromXML(model, research);

                return model;
            }
        }

        private SearchModel GetArticlesFromXML (SearchModel model, IList<Curriculos> researchs, bool countCoAuthors = false)
        {
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DADOS-BASICOS-DO-ARTIGO";

            if (model.articles == null)
                model.articles = new List<ArticleModel>();
            foreach (var author in researchs)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data);
                
                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    var article = new ArticleModel();
                    article.AuthorId = author.Id;
                    article.Id = Convert.ToInt32(node.NextSibling.Attributes["ISSN"].Value);
                    article.Author = author.Name;
                    article.Nature = node.Attributes["NATUREZA"].Value;
                    article.Title = node.Attributes["TITULO-DO-ARTIGO"].Value;
                    article.Year = Convert.ToInt32(node.Attributes["ANO-DO-ARTIGO"].Value);
                    article.Country = node.Attributes["PAIS-DE-PUBLICACAO"].Value;
                    article.Language = node.Attributes["IDIOMA"].Value;
                    article.HomePage = node.Attributes["HOME-PAGE-DO-TRABALHO"].Value;
                    article.Relevant = node.Attributes["FLAG-RELEVANCIA"].Value;
                    article.Doi = node.Attributes["DOI"].Value;
                    article.EnglishTitle = node.Attributes["TITULO-DO-ARTIGO-INGLES"].Value;
                    article.Revelation = node.Attributes["FLAG-DIVULGACAO-CIENTIFICA"].Value;
                    article.Coauthors = node.ParentNode.SelectNodes("AUTORES").Count - 1; //Theres is gonna be always one main author
                    model.articles.Add(article);
                }
            }

            return model;
        }

        private Curriculos GetResearchByName(string name)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Name.Equals(name)).FirstOrDefault();
            }
        }

        private Curriculos GetResearchById(int id)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Id.Equals(id)).FirstOrDefault();
            }
        }

        private IList<Curriculos> GetAll()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.ToList();
            }
        }

        private IList<Curriculos> GetAllWorking()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Working == true).ToList();
            }
        }
    }
}
