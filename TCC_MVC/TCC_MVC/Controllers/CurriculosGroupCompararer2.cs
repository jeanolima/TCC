using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCC_MVC.Models
{
    public class CurriculosGroupComparer2 : IEqualityComparer<CurriculosGroup>
    {
        public bool Equals(CurriculosGroup x, CurriculosGroup y)
        {
            return x.CurriculoId == y.CurriculoId;
        }

        public int GetHashCode(CurriculosGroup obj)
        {
            return obj.CurriculoId.GetHashCode();
        }
    }
}
