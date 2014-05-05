using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class CurriculoComparer : IEqualityComparer<CurriculoModel>
    {
        public bool Equals(CurriculoModel x, CurriculoModel y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(CurriculoModel obj)
        {
            return obj.Id.GetHashCode();
        }

    }
}
