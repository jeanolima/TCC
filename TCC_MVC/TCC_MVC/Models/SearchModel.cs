﻿using System;
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
        public int Total { get; set; }
        public int TotalWithQuallis { get; set; }
        public int TotalWithoutQuallis { get; set; }
        public int TotalSpecificQuallis { get; set; }
        public int TotalA1 { get; set; }
        public int TotalA2 { get; set; }
        public int TotalB1 { get; set; }
        public int TotalB2 { get; set; }
        public int TotalB3 { get; set; }
        public int TotalB4 { get; set; }
        public int TotalB5 { get; set; }
        public int TotalC { get; set; }
        public IList<YearArticlesModel> Years { get; set; }
    }
}
