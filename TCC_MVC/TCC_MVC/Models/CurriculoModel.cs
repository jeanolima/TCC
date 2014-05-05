using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class CurriculoModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public System.DateTime UpdatedIn { get; set; }
        public bool Working { get; set; }
        public bool IsSelected { get; set; }
    }
}
