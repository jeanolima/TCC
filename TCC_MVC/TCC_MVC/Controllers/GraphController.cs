using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;
using System.Text;
using System.Xml.Linq;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class GraphController : Controller
    {
        public ActionResult Index()
        {
            using(var Context = new TCC_LUCASEntities())
            {
                //var item = Context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements("x").Single().Value == "1");
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//IDIOMAS//IDIOMA[@IDIOMA='FR']";
                var item = Context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any());
                var item2 = item.ToList();

            }
            //StringBuilder sbDoc = new StringBuilder();
            //XPathDocument doc = new XPathDocument(@"C:\Users\Usuario\Desktop\TCC_Lucas\branches\MVC\Curriculos\curriculo(1).xml");
            //XPathNavigator nav = doc.CreateNavigator();
            //XPathExpression expr;
            //expr = nav.Compile("/CURRICULO-VITAE/DADOS-GERAIS/IDIOMAS/IDIOMA");
            //XPathNodeIterator iterator = nav.Select(expr);

            //while (iterator.MoveNext())
            //{
            //    sbDoc.Append(iterator.Current.OuterXml);
            //}

            //ViewBag.Retorno = sbDoc.ToString();

            return View();
        }
    }
}
