using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class EvolutionGraphicModel 
    {
        public IList<EvolutionModel> Seasons { get; set; }
        public string EvolutionType { get; set; }
        public int EvolutionGap { get; set; }

    }
}
