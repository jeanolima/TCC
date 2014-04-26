using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TCC_MVC.Models
{
    public class SearchModel 
    {
        public IList<ArticleModel> articles { get; set;}
        public IList<Curriculos> curriculos { get; set;}
        public int Total { get; set; }
        public QualisModel Qualis { get; set; }
        public bool showQualis { get; set; }
        public IList<YearArticlesModel> Years { get; set; }

        [Display(Name = "Palavra Chave")]
        public string Keyword { get; set; }
        [Display(Name = "Pesquisar por")]
        public string KeyType { get; set; }
        
        [Display(Name = "Ordenar por")]
        public string OrderByType { get; set; }

        [Display(Name = "Agrupar por")]
        public string GroupByType { get; set; }
        
        public string EvolutionType { get; set; }
        public int EvolutionGap { get; set; }
        public EvolutionGraphicModel EvolutionGrafic { get; set; }
    }
}
