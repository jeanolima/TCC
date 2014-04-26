using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class QualisModel 
    {
        public bool hasQualis { get; set; }
        public bool withCheck { get; set; }
        public int TotalWithQuallis { get; set; }
        public bool withoutCheck { get; set; }
        public int TotalWithoutQuallis { get; set; }
        public bool specificCheck { get; set; }
        public int TotalSpecificQuallis { get; set; }
        public bool a1Check { get; set; }
        public int TotalA1 { get; set; }
        public bool a2Check { get; set; }
        public int TotalA2 { get; set; }
        public bool b1Check { get; set; }
        public int TotalB1 { get; set; }
        public bool b2Check { get; set; }
        public int TotalB2 { get; set; }
        public bool b3Check { get; set; }
        public int TotalB3 { get; set; }
        public bool b4Check { get; set; }
        public int TotalB4 { get; set; }
        public bool b5Check { get; set; }
        public int TotalB5 { get; set; }
        public bool cCheck { get; set; }
        public int TotalC { get; set; }
    }
}
