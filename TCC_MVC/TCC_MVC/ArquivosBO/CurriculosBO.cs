using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TCC_MVC.Models;

namespace TCC_MVC.ArquivosBO
{
    public class CurriculosBO
    {
        public QualisModel CountAllQualis (SearchModel model)
        {
            var qualis = model.Qualis;
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DETALHAMENTO-DO-ARTIGO";
            string xpathEvent = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//DETALHAMENTO-DO-TRABALHO";

            using (var _context = new TCC_LUCASEntities())
            {
                foreach (var author in model.curriculos)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(author.Data);

                    foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                    {
                        string title = node.Attributes["TITULO-DO-PERIODICO-OU-REVISTA"].Value;
                        var ConferenceQualis = _context.QualisPeriodico.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis != null)
                            total++;
                    }
                    foreach (XmlNode node in xml.SelectNodes(xpathEvent))
                    {
                        string title = node.Attributes["NOME-DO-EVENTO"].Value;
                        var ConferenceQualis = _context.QualisConference.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis != null)
                            total++;
                    }
                }
            }
            qualis.TotalWithQuallis = total;
            return qualis;
        }

        public QualisModel CountAllWithoutQualis(SearchModel model)
        {
            var qualis = model.Qualis;
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DETALHAMENTO-DO-ARTIGO";
            string xpathEvent = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//DETALHAMENTO-DO-TRABALHO";

            using (var _context = new TCC_LUCASEntities())
            {
                foreach (var author in model.curriculos)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(author.Data);

                    foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                    {
                        string title = node.Attributes["TITULO-DO-PERIODICO-OU-REVISTA"].Value;
                        var ConferenceQualis = _context.QualisPeriodico.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis == null)
                            total++;
                    }
                    foreach (XmlNode node in xml.SelectNodes(xpathEvent))
                    {
                        string title = node.Attributes["NOME-DO-EVENTO"].Value;
                        var ConferenceQualis = _context.QualisConference.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis == null)
                            total++;
                    }
                }
            }
            qualis.TotalWithoutQuallis = total;
            return qualis;
        }
        
        public QualisModel CountSpecificQualis(SearchModel model, string type)
        {
            var qualis = model.Qualis;
            int total = 0;
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DETALHAMENTO-DO-ARTIGO";
            string xpathEvent = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//DETALHAMENTO-DO-TRABALHO";
            using (var _context = new TCC_LUCASEntities())
            {
                foreach (var author in model.curriculos)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(author.Data);

                    foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                    {
                        string title = node.Attributes["TITULO-DO-PERIODICO-OU-REVISTA"].Value;
                        var ConferenceQualis = _context.QualisPeriodico.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis != null && ConferenceQualis.Type.ToLower().Equals(type))
                            total++;
                    }
                    foreach (XmlNode node in xml.SelectNodes(xpathEvent))
                    {
                        string title = node.Attributes["NOME-DO-EVENTO"].Value;
                        var ConferenceQualis = _context.QualisConference.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis != null && ConferenceQualis.Type.ToLower().Equals(type))
                            total++;
                    }
                }
            }
            switch(type)
            {
                case "a1": { qualis.TotalA1 = total; break; }
                case "a2": { qualis.TotalA2 = total; break; }
                case "b1": { qualis.TotalB1 = total; break; }
                case "b2": { qualis.TotalB2 = total; break; }
                case "b3": { qualis.TotalB3 = total; break; }
                case "b4": { qualis.TotalB4 = total; break; }
                case "b5": { qualis.TotalB5 = total; break; }
                case "c" : { qualis.TotalC  = total; break; }
            }
            return qualis;
        }

        //não esta pronta
        public QualisModel CountAllSpecificQualis(SearchModel model)
        {
            var allQualis = model.Qualis;
            using (var _context = new TCC_LUCASEntities())
            {
                string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DETALHAMENTO-DO-ARTIGO";
                string xpathEvent = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//TRABALHOS-EM-EVENTOS//TRABALHO-EM-EVENTOS//DETALHAMENTO-DO-TRABALHO";
                    
                foreach (var author in model.curriculos)
                {
                    
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(author.Data);
                    
                    foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                    {
                        string title = node.Attributes["TITULO-DO-PERIODICO-OU-REVISTA"].Value;
                        var ConferenceQualis = _context.QualisPeriodico.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis != null)
                        {
                            switch (ConferenceQualis.Type.ToLower())
                            {
                                case "a1": { allQualis.TotalA1++; break; }
                                case "a2": { allQualis.TotalA2++; break; }
                                case "b1": { allQualis.TotalB1++; break; }
                                case "b2": { allQualis.TotalB2++; break; }
                                case "b3": { allQualis.TotalB3++; break; }
                                case "b4": { allQualis.TotalB4++; break; }
                                case "b5": { allQualis.TotalB5++; break; }
                                case "c": { allQualis.TotalC++; break; }
                                default: { break; }
                            }
                        }
                    }

                    foreach (XmlNode node in xml.SelectNodes(xpathEvent))
                    {
                        string title = node.Attributes["NOME-DO-EVENTO"].Value;
                        var ConferenceQualis = _context.QualisConference.Where(x => x.Conference.Equals(title)).FirstOrDefault();
                        if (ConferenceQualis != null)
                        {
                            switch (ConferenceQualis.Type.ToLower())
                            {
                                case "a1": { allQualis.TotalA1++; break; }
                                case "a2": { allQualis.TotalA2++; break; }
                                case "b1": { allQualis.TotalB1++; break; }
                                case "b2": { allQualis.TotalB2++; break; }
                                case "b3": { allQualis.TotalB3++; break; }
                                case "b4": { allQualis.TotalB4++; break; }
                                case "b5": { allQualis.TotalB5++; break; }
                                case "c": { allQualis.TotalC++; break; }
                                default: { break; }
                            }
                        }
                    }
                }
            }
            return allQualis;
        }

        public SearchModel GroupByCompleteType(SearchModel model)
        {
            model.articles = model.articles.Where(x => x.Nature.ToLower().Equals("completo")).ToList();
            return model;
        }

        public SearchModel GroupByResumeType(SearchModel model)
        {
            model.articles = model.articles.Where(x => x.Nature.ToLower().Equals("resumo")).ToList();
            return model;
        }

        public SearchModel OrderByTriennium(SearchModel model)
        {
            var yearNow = DateTime.Now.Year;
            int triennium = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Triennium"]);
            model.articles = model.articles.Where(x => (x.Year > yearNow - triennium ? true : false)).OrderBy(x => x.Year).ToList();

            var years = model.articles.Select(x => x.Year).Distinct().ToList();
            model.Years = new List<YearArticlesModel>();

            foreach (var year in years)
            {
                var totalOfYear = model.articles.Where(x => x.Year.Equals(year)).Count();

                model.Years.Add(new YearArticlesModel
                {
                    TotalArticles = totalOfYear,
                    Year = year
                });
            }

            return model;
        }

        public SearchModel OrderByYear (SearchModel model)
        {
            model.articles = model.articles.OrderBy(x => x.Year).ToList();

            var years = model.articles.Select(x => x.Year).Distinct().ToList();
            model.Years = new List<YearArticlesModel>();
            
            foreach (var year in years)
            {
                var totalOfYear = model.articles.Where(x => x.Year.Equals(year)).Count();

                model.Years.Add(new YearArticlesModel{
                    TotalArticles = totalOfYear,
                    Year = year
                });
            }

            return model;
        }
        
        public IList<ArticleModel> OrderByEvolution(SearchModel model)
        {
            int seasonGap = Int32.Parse(ConfigurationManager.AppSettings["Triennium"]) * model.EvolutionGap;
            int year = DateTime.Now.Year;
            int select = (model.EvolutionType.Equals("year") ? year - model.EvolutionGap : year - seasonGap * model.EvolutionGap);

            return model.articles.Where(x => x.Year >= select).OrderBy(x => x.Year).ToList(); ;
        }

        public EvolutionGraphicModel CountByEvolution(SearchModel model)
        {
            var evolutionGraphic = new EvolutionGraphicModel();
            int seasonGap = Int32.Parse(ConfigurationManager.AppSettings["Triennium"]) * model.EvolutionGap; 
            int year = DateTime.Now.Year;
            int select = (model.EvolutionType.Equals("year") ? year - model.EvolutionGap : year - seasonGap * model.EvolutionGap);

            evolutionGraphic.EvolutionType = (model.EvolutionType.Equals("year") ? "anos" : "trienos");
            evolutionGraphic.EvolutionGap = model.EvolutionGap;
            evolutionGraphic.Seasons = model.articles.Where(x => x.Year >= select).Select(x => x.Year).Distinct().Select(t => new EvolutionModel 
            {
                year = t,
                total = model.articles.Where(x => x.Year.Equals(t)).Count()
            }).ToList();

            return evolutionGraphic;
        }

        public SearchModel GetAllArticles()
        {
            var model = new SearchModel();
            var researches = GetAll();
            model.curriculos =researches;
            model = GetArticlesFromXML(model, researches, true);

            return model;
        }

        public SearchModel GetResearchsByAuthor(SearchModel model, string name)
        {
            
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS[contains(translate(@NOME-COMPLETO,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + name.ToLower() + "') or translate(@NOME-COMPLETO,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" + name.ToLower() + "']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model = GetArticlesFromXML(model, research);

                return model;
            }
        }


        public SearchModel GetResearchBySearch(SearchModel model, string title)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//ATUACOES-PROFISSIONAIS//ATUACAO-PROFISSIONAL//LINHA-DE-PESQUISA[contains(translate(@TITULO-DA-LINHA-DE-PESQUISA,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + title.ToLower() + "') or translate(@TITULO-DA-LINHA-DE-PESQUISA,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" + title.ToLower() + "']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model = GetArticlesFromXML(model, research);

                return model;
            }
        }

        public SearchModel GetResearchByGroup(SearchModel model, string projectName)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                string xpath = "//CURRICULO-VITAE//DADOS-GERAIS//ATUACOES-PROFISSIONAIS//ATUACAO-PROFISSIONAL//PROJETO-DE-PESQUISA[contains(translate(@NOME-DO-PROJETO,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + projectName.ToLower() + "') or translate(@NOME-DO-PROJETO,'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" + projectName.ToLower() + "']";
                var research = _context.Curriculos.AsEnumerable().Where(entry => entry.Id == 13 && XDocument.Parse("<Root>" + entry.Data + "</Root>").XPathSelectElements(xpath).Any()).ToList();

                model.curriculos = research;
                model = GetArticlesFromXML(model, research);

                return model;
            }
        }

        public SearchModel GetResearchByGroup(SearchModel model, int idGroup)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                model.curriculos = (from c in _context.Curriculos
                           join cg in _context.CurriculosGroup on c.Id equals cg.CurriculoId
                           where cg.GroupId.Equals(idGroup)
                            select c).ToList();
                model = GetArticlesFromXML(model, model.curriculos);

                return model;
            }
        }

        public SearchModel GetArticlesFromXML (SearchModel model, IList<Curriculos> researchs, bool countCoAuthors = false)
        {
            string xpathArticle = "//CURRICULO-VITAE//PRODUCAO-BIBLIOGRAFICA//ARTIGOS-PUBLICADOS//ARTIGO-PUBLICADO//DADOS-BASICOS-DO-ARTIGO";

            if (model.articles == null)
                model.articles = new List<ArticleModel>();
            foreach (var author in researchs)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(author.Data);
                
                foreach (XmlNode node in xml.SelectNodes(xpathArticle))
                {
                    var article = new ArticleModel();
                    article.AuthorId = author.Id;
                    article.Id = (!string.IsNullOrEmpty(node.NextSibling.Attributes["ISSN"].Value) ? Convert.ToInt32(Regex.Match(node.NextSibling.Attributes["ISSN"].Value, @"\d+").Value) : 0);
                    article.Author = author.Name;
                    article.Nature = node.Attributes["NATUREZA"].Value;
                    article.Title = node.Attributes["TITULO-DO-ARTIGO"].Value;
                    article.Year = (!string.IsNullOrEmpty(node.Attributes["ANO-DO-ARTIGO"].Value) ? Convert.ToInt32(Regex.Match(node.Attributes["ANO-DO-ARTIGO"].Value, @"\d+").Value) : 0);
                    article.Country = node.Attributes["PAIS-DE-PUBLICACAO"].Value;
                    article.Language = node.Attributes["IDIOMA"].Value;
                    article.HomePage = node.Attributes["HOME-PAGE-DO-TRABALHO"].Value;
                    article.Relevant = node.Attributes["FLAG-RELEVANCIA"].Value;
                    article.Doi = node.Attributes["DOI"].Value;
                    article.EnglishTitle = node.Attributes["TITULO-DO-ARTIGO-INGLES"].Value;
                    article.Revelation = node.Attributes["FLAG-DIVULGACAO-CIENTIFICA"].Value;
                    article.Coauthors = node.ParentNode.SelectNodes("AUTORES").Count - 1; //Theres is gonna be always one main author
                    model.articles.Add(article);
                }
            }

            return model;
        }

        public Curriculos GetResearchByName(string name)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Name.Equals(name)).FirstOrDefault();
            }
        }

        public Curriculos GetResearchById(int id)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Id.Equals(id)).FirstOrDefault();
            }
        }

        public IList<Curriculos> GetAll()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.ToList();
            }
        }

        public IList<Curriculos> GetAllWorking()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Working == true).ToList();
            }
        }
    }
}