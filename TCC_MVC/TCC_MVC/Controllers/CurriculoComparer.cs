using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class CurriculoComparer : IEqualityComparer<Curriculos>
    {
        public bool Equals(Curriculos x, Curriculos y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Curriculos obj)
        {
            return obj.Id.GetHashCode();
        }

    }
}
