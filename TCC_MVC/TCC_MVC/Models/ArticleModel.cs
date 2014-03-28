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

        public string Nature { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }

        public string HomePage { get; set; }

        public string Relevant { get; set; }

        public string Doi { get; set; }

        public string EnglishTitle { get; set; }

        public string Revelation { get; set; }

        public int Year { get; set; }

        public int Coauthors { get; set; }
    }
}
