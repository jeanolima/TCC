using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class LineModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<AreaModel> Areas { get; set; }
        public int AreaSelected { get; set; }
        public IList<CurriculoModel> Researchs { get; set; }
    }
}
