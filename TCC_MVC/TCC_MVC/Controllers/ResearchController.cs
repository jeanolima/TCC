using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.ArquivosBO;
using TCC_MVC.Models;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace TCC_MVC.Controllers
{
    public class ResearchController : Controller
    {
        CurriculosBO _curriculosBO = new CurriculosBO();

        public ActionResult Index()
        {
            ViewBag.List = _curriculosBO.GetAllWorking();
            return View();
        }

        
        public ActionResult New()
        {
            Curriculos model = new Curriculos();
            ViewBag.Action = "New";
            ViewBag.Research = "Research";

            return View("_Form");
        }

        [HttpPost]
        public ActionResult New(Curriculos model)
        {
            if (!Validate(model))
                ModelState.AddModelError("Nome", "O nome informado não é o mesmo do curriculo submetido. Tem certeza que usaste o currículo certo?");
            if (model.Data != null)
            {
                if (!SaveResearch(model))
                    ModelState.AddModelError("error", "Não foi possível fazer upload do documento. Por favor tente mais tarde");
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int idAuthor)
        {
            ViewBag.Action = "Edit";
            ViewBag.Research = "Research";
            var model = _curriculosBO.GetResearchById(idAuthor);
            
            return View("_Form", model);
        }

        [HttpPost]
        public ActionResult Edit(Curriculos model)
        {
            HttpPostedFileBase file = Request.Files[0];
            if(!Validate(model))
                ModelState.AddModelError("Nome", "O nome informado não é o mesmo do curriculo submetido. Tem certeza que usaste o currículo certo?");
            if (model.Data != null)
            {
                if (!SaveResearch(model))
                    ModelState.AddModelError("upload", "Não foi possível fazer upload do documento. Por favor tente mais tarde");
            }

            return RedirectToAction("Index");
        }

        private bool Validate(Curriculos model)
        {
            //To validate whether the name form model and from xml are the same
            return true;
        }

        [HttpPost]
        protected bool SaveResearch(Curriculos model)
        {
            HttpPostedFileBase file = Request.Files[0];
            string name = model.Name;
            bool working = model.Working;
            DateTime updateIn = DateTime.Now;
            using (Stream fs = file.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string constr = ConfigurationManager.ConnectionStrings["ConnectionWithoutEF"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "";
                        if(model.Id == null || model.Id == 0)
                            query = "INSERT INTO Curriculos(Data, Working, UpdatedIn, Name) VALUES (@Data, @Working, @UpdatedIn, @Name)";
                        else
                            query = "UPDATE Curriculos SET [Data] = @Data, [Working] = @Working, [UpdatedIn] = @UpdatedIn, [Name] = @Name WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@Data", bytes);
                            cmd.Parameters.AddWithValue("@Working", working);
                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@UpdatedIn", DateTime.Now);
                            if (model.Id != 0)
                                cmd.Parameters.AddWithValue("@Id", model.Id);
                            
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            return true;
                           
                        }
                    }
                }
            }
        }

    }
}
