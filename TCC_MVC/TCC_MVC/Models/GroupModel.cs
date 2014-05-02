using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class GroupModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Curriculos> Researchs { get; set; }
        public IList<Curriculos> ResearchsOut { get; set; }

        public IList<bool> ResearchsLinked { get; set; }
        public IList<int> ResearchsLinkedIds { get; set; }
    }
}
