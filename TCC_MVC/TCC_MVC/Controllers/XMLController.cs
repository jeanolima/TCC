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
                //var item = Context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements("x").Single().Value == "1");
                //string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//IDIOMAS//IDIOMA[@IDIOMA='FR']";
                //var item = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any());
                //var item2 = item.ToList();

                var temp = GetResearchsByAuthor("Valter");
                ViewBag.Lista = temp;
            }

            return View();
        }

        private IList<string> GetResearchsByAuthor(string name)
        {
            
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS[contains(@NOME-COMPLETO,'" + name + "')]";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                var articles = new List<string>();
                string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DADOS-BASICOS-DO-ARTIGO";
                foreach (var author in research)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(author.Data); // Load(file)

                    foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                    {
                        articles.Add(node.Attributes["TITULO-DO-ARTIGO"].Value);
                    }
                }

                return articles;
            }
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

        private IList<Curriculos> GetResearchBySearch(string title)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//ATUACOES-PROFISSIONAIS//ATUACAO-PROFISSIONAL//LINHA-DE-PESQUISA[contains (@TITULO-DA-LINHA-DE-PESQUISA, '" + title + "')]";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();
                return research;
            }
        }

        private IList<Curriculos> GetResearchByGroup(string projectName)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//ATUACOES-PROFISSIONAIS//ATUACAO-PROFISSIONAL//ATIVIDADE-DE-PARTICIPACAO-EM-PROJETO//PARTICIPACAO-EM-PROJETO//PROJETO-DE-PESQUISA [contains (@NOME-DE-PROJETO, '" + projectName + "')]";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();
                return research;
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
