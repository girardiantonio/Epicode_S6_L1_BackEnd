using Epicode_S6_L1_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Epicode_S6_L1_BackEnd.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UtenteController : Controller
    {

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        private Utente GetUtenteById(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Utenti WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Utente utente = new Utente
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                LuogoDiNascita = reader["LuogoDiNascita"].ToString(),
                                Residenza = reader["Residenza"].ToString(),
                                DataDiNascita = (DateTime)reader["DataDiNascita"]
                            };
                            return utente;
                        }
                        return null;
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult ListaUtente()
        {
            List<Utente> utenti = new List<Utente>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Utenti";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Utente utente = new Utente
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                LuogoDiNascita = reader["LuogoDiNascita"].ToString(),
                                Residenza = reader["Residenza"].ToString(),
                                DataDiNascita = (DateTime)reader["DataDiNascita"]
                            };

                            utenti.Add(utente);
                        }
                    }
                }

            }
            return View(utenti);
        }

        [HttpGet] 
        public ActionResult AggiungiUtente()
        { 
            return View(); 
        }

        [HttpPost]
        public ActionResult AggiungiUtente(Utente model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Utenti (Nome, Cognome, CodiceFiscale, LuogoDiNascita, Residenza, DataDiNascita)" + "VALUES (@Nome, @Cognome, @CodiceFiscale, @LuogoDiNascita, @Residenza, @DataDiNascita)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", model.Nome);
                        cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                        cmd.Parameters.AddWithValue("@CodiceFiscale", model.CodiceFiscale);
                        cmd.Parameters.AddWithValue("@LuogoDiNascita", model.LuogoDiNascita);
                        cmd.Parameters.AddWithValue("@Residenza", model.Residenza);
                        cmd.Parameters.AddWithValue("@DataDiNascita", model.DataDiNascita);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Utente aggiunto con successo!";
                return RedirectToAction("ListaUtente");
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaUtente(int Id)
        {
            Utente utenteDaEliminare = GetUtenteById(Id);

            if (utenteDaEliminare == null)
            {
                TempData["Errore"] = "Utente non trovato!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Utenti WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Utente eliminato con successo!";
            }
            return RedirectToAction("ListaUtente");
        }

        [HttpGet]
        public ActionResult ModificaUtente(int Id)
        {
            Utente utenteDaModificare = GetUtenteById(Id);

            if (utenteDaModificare == null)
            {
                TempData["Errore"] = "Trasgressore non trovato!";
            }

            return View(utenteDaModificare);
        }

        [HttpPost]
        public ActionResult ModificaUtente(Utente utenteModificato)
        {
            if (ModelState.IsValid)
            {
                string query = "UPDATE Utenti SET " +
                    "Nome = @Nome, " +
                    "Cognome = @Cognome, " +
                    "CodiceFiscale = @CodiceFiscale, " +
                    "LuogoDiNascita = @LuogoDiNascita, " +
                    "Residenza = @Residenza, " +
                    "DataDiNascita = @DataDiNascita " +
                    "WHERE Id = @Id";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Id", utenteModificato.Id);
                        cmd.Parameters.AddWithValue("@Nome", utenteModificato.Nome);
                        cmd.Parameters.AddWithValue("@Cognome", utenteModificato.Cognome);
                        cmd.Parameters.AddWithValue("@CodiceFiscale", utenteModificato.CodiceFiscale);
                        cmd.Parameters.AddWithValue("@LuogoDiNascita", utenteModificato.LuogoDiNascita);
                        cmd.Parameters.AddWithValue("@Residenza", utenteModificato.Residenza);
                        cmd.Parameters.AddWithValue("@DataDiNascita", utenteModificato.DataDiNascita);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Utente modificato con successo!";
            }
            return RedirectToAction("ListaUtente");
        }

    }
}