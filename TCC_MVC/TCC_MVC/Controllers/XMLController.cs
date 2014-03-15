using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

                var temp = GetResearchByNameInXML("er");
                ViewBag.Lista = temp;
            }

            return View();
        }

        private IList<Curriculos> GetResearchByNameInXML(string name)
        {
            
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS[contains(@NOME-COMPLETO,'" + name + "')]";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();
                return research;
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
