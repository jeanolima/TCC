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
                var temp = GetResearchByGroup(model, "Transporte para o SBTVD");
                ViewBag.Lista = temp;
                ViewBag.Total = GetAllArticles().Count();
            }

            return View();
        }

        private SearchModel OrderByYear (SearchModel model)
        {


            return model;
        }

        private IList<string> GetAllArticles()
        {
            var researches = GetAll();
            var articles = GetArticlesFromXML(researches);

            return articles;
        }

        private SearchModel GetResearchsByAuthor(SearchModel model, string name)
        {
            
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS[contains(@NOME-COMPLETO,'" + name + "') or @NOME-COMPLETO= '" + name + "']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model.articles = GetArticlesFromXML(research).Select(x => new ArticleModel { 
                   Title = x 
                }).ToList();

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
                model.articles = GetArticlesFromXML(research).Select(x => new ArticleModel
                {
                    Title = x
                }).ToList();

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
                model.articles = GetArticlesFromXML(research).Select(x => new ArticleModel
                {
                    Title = x
                }).ToList();

                return model;
            }
        }

        private IList<string> GetArticlesFromXML (IList<Curriculos> researchs)
        {

            var articles = new List<string>();
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DADOS-BASICOS-DO-ARTIGO";
            foreach (var author in researchs)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data); // Load(file)

                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    articles.Add(node.Attributes["TITULO-DO-ARTIGO"].Value);

                    //popular a model nesta parte do código
                }
            }

            return articles;
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
