using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.Models;

namespace TCC_MVC.Controllers
{
    public class CurriculosLineComparer : IEqualityComparer<CurriculosLine>
    {
        public bool Equals(CurriculosLine x, CurriculosLine y)
        {
            return x.CurriculoId == y.CurriculoId;
        }

        public int GetHashCode(CurriculosLine obj)
        {
            return obj.CurriculoId.GetHashCode();
        }
    }
}
