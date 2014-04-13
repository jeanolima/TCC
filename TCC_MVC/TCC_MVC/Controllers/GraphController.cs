using System;
using System.Collections.Generic;
using System.Web.Mvc;
//using Satsuma;

namespace TCC_MVC.Controllers
{
    public class GraphController : Controller
    {
        public ActionResult Index()
        {
            //CustomGraph z = new CustomGraph();

            //Node a = z.AddNode();
            //Node b = z.AddNode();
            //Node c = z.AddNode();
            //Arc ab = z.AddArc(a, b, Directedness.Directed);
            //Arc bc = z.AddArc(b, c, Directedness.Undirected);

            //var g = new CompleteGraph(100, Directedness.Directed); // create a complete graph on 100 nodes
            //var cost = new Dictionary<Node, double>(); // create a cost function on the nodes
            //int i = 0;
            //foreach (Node node in g.Nodes()) cost[node] = i++; // assign some integral costs to the nodes
            //Func<Arc, double> arcCost =
            //    (arc => cost[g.U(arc)] + cost[g.V(arc)]); // a cost of an arc will be the sum of the costs of the two nodes
            //foreach (Arc arc in g.Arcs())
            //    Console.WriteLine("Cost of " + g.ArcToString(arc) + " is " + arcCost(arc));

            return View();
        }
    }
}
