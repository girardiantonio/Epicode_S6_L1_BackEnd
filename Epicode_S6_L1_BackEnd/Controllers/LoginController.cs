using Epicode_S6_L1_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Epicode_S6_L1_BackEnd.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                try
                {
                    sqlConnection.Open();

                    string query = "SELECT * FROM Login WHERE Username = @Username AND Password = @Password";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("Username", model.Username);
                    sqlCommand.Parameters.AddWithValue("Password", model.Password);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.AuthError = "Autenticazione non riuscita, credenziali non corrette";
                        return View();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register([Bind(Exclude ="IsAdmin")] Login model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Login (Nome, Cognome, Email, Username, Password)" + "VALUES (@Nome, @Cognome, @Email, @Username, @Password)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", model.Nome);
                        cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Username", model.Username);
                        cmd.Parameters.AddWithValue("@Password", model.Password);

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.AuthError = "Registrazione non riuscita, moduli non corretti";
            return View(model);
        }
    }
}