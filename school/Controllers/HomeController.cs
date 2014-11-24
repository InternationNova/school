using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Classes;
using school.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace school.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private GlobalInfo db = new GlobalInfo();
        HttpCookie _userInfoCookies = new HttpCookie("UserInfo");
        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly Random _rng = new Random();
        

        public ActionResult Index()
        {
            HttpCookie _userInfoCookiesLogin = Request.Cookies["UserInfo"];
            if (_userInfoCookiesLogin != null)
            {
                Session["USERNAME"] = _userInfoCookiesLogin["USERNAME"];
                Session["PASSWORD"] = _userInfoCookiesLogin["PASSWORD"];
                return RedirectToAction("main", "Home");
                
            }
            return View();
        }
        public string GetStatus()
        {
            return "Status OK at " + DateTime.Now.ToLongTimeString();
        }
        public ActionResult login(string username, string password, Boolean remember)
        {
            int fail_number = 0;
            string str_query = @"SELECT * FROM usuarios WHERE usuario =@username AND senha = @password and is_blocked = 0";
            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
              
                // byte array representation of that string
                byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

                string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", encoded);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Session["USERNAME"] = username;
                    Session["PASSWORD"] = encoded;

                    if (remember == true) {
                        
                        
                        _userInfoCookies["USERNAME"] = Session["USERNAME"].ToString();
                        _userInfoCookies["PASSWORD"] = Session["PASSWORD"].ToString();
                        _userInfoCookies.Expires = DateTime.Now.AddDays(5);

                        Response.Cookies.Add(_userInfoCookies);
                    }

                }
                if (reader.Read() == false) {
                    
                    usuarios usuariosObj = new usuarios();
                    
                    var query =
                        from ord in db.usuarios
                        where ord.usuario == username
                        select ord;
                    
                    foreach (usuarios ord in query)
                    {

                        ord.fail_number = ord.fail_number + 1;
                        fail_number = ord.fail_number;
                        if (fail_number == 5) {
                            ord.is_blocked = true;
                        }
                        // Insert any additional changes to column values.
                    }

                    // Submit the changes to the database. 
                    try
                    {
                        db.SaveChanges();
                        if (fail_number == 5) {
                            conn.Close();
                            return Json(new { ok = "blocked" });
                        
                        }
                        
                    }
                    catch (Exception e)
                    {
                       
                    }
                
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

            _userInfoCookies.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(_userInfoCookies);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult main() {
            return View();   
        }
        public void setPassword(string password, string emailAddress)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            string str_query = @"update usuarios set senha = @password where email = @email";


            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                cmd.Parameters.AddWithValue("@password", encoded);
                cmd.Parameters.AddWithValue("@email", emailAddress);
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                
             }

           
        }

        /* Thread Main Class */
        private string RandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        public ActionResult sendmail(string emailAddress)
        {
            string emailAdd = "";
            string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;
            string str_query = @"SELECT * FROM usuarios WHERE email = @email";
            using (SqlConnection conn= new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@email", emailAddress);
                conn.Open();
            
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        emailAdd = reader["email"].ToString();
                    }

                conn.Close();
            }
            if (emailAdd != "")
            {
                string passString = RandomString(8);
                setPassword(passString, emailAdd);
                try
                {
                    var fromAddress = new MailAddress("sostecoinfo@gmail.com", "Sosteco Info Contadores");
                    var toAddress = new MailAddress(emailAddress, "");
                    const string fromPassword = "lolazo123";


                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = "Cambio contraseña",
                        Body = "Su contraseña es : " + passString
                    })
                    {
                        smtp.Send(message);
                        return Json(new { ok = "success" });
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { ok = "mailfailed" });
                }


            }
            else
            {

                return Json(new { ok = "userfailed" });
            }
        }

    }   
}
