using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class XMLController : Controller
    {
        //
        // GET: /XML/

        public ActionResult Index()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                var model = new SearchModel();
                model = GetAllArticles();
                ViewBag.All = CountAllQualis(model);
                ViewBag.Without = CountAllWithoutQualis(model);
                ViewBag.Specific = CountSpecificQualis(model, "A1");
            }

            return View();
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
        private int CountAllSpecificQualis(SearchModel model)
        {
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//INFORMACOES-ADICIONAIS";
            //var qualis = 
            foreach (var author in model.curriculos)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data);

                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    var info = node.Attributes["DESCRICAO-INFORMACOES-ADICIONAIS"].Value;
                    if (info.Contains("") ||
                        !info.Contains("Nada consta") ||
                        !info.Contains("Não está classificado"))
                    {
                        total++;
                    }
                }
            }

            return total;
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
            model = GetArticlesFromXML(model, researches);

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

        private SearchModel GetArticlesFromXML (SearchModel model, IList<Curriculos> researchs)
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
