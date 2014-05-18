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
        public float Id { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string QualisName { get; set; }
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
        public string Qualis { get; set; }
        public bool IsArticle { get; set; }
    }
}
