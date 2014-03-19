using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class SearchModel 
    {
        public IList<ArticleModel> articles { get; set;}

        public IList<Curriculos> curriculos { get; set;}
    }
}
