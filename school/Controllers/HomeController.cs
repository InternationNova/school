using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Classes;
using school.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace school.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private GlobalInfo db = new GlobalInfo();

        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }
        public string GetStatus()
        {
            return "Status OK at " + DateTime.Now.ToLongTimeString();
        }
        public ActionResult login(string username, string password)
        {
            string str_query = @"SELECT * FROM usuarios WHERE usuario =@username AND senha = @password";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Session["USERNAME"] = username;

                }

                conn.Close();
                if (Session["USERNAME"] == null)
                {
                    return Json(new { ok = "failed" });

                }
                else
                {
                    return Json(new { ok = "success" });
                }


            }
        }
        public ActionResult logout()
        {
            Session.Remove("USERNAME");
            return RedirectToAction("Index", "Home");
        }
    }
}
