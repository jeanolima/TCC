using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class ArticleModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Year { get; set; }
    }
}
