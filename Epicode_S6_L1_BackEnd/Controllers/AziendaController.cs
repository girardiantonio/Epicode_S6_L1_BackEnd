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
    public class AziendaController : Controller
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        private Azienda GetAziendaById(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Azienda WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Azienda azienda = new Azienda
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                PartitaIVA = reader["PartitaIVA"].ToString(),
                                IndirizzoSede = reader["IndirizzoSede"].ToString(),
                                CittaSede = reader["CittaSede"].ToString(),
                                Gestione = reader["Gestione"].ToString(),
                            };
                            return azienda;
                        }
                        return null;
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult ListaAzienda()
        {
            List<Azienda> aziende = new List<Azienda>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Azienda";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Azienda azienda = new Azienda
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                PartitaIVA = reader["PartitaIVA"].ToString(),
                                IndirizzoSede = reader["IndirizzoSede"].ToString(),
                                CittaSede = reader["CittaSede"].ToString(),
                                Gestione = reader["Gestione"].ToString(),
                            };

                            aziende.Add(azienda);
                        }
                    }
                }

            }
            return View(aziende);
        }

        [HttpGet]
        public ActionResult AggiungiAzienda()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiAzienda(Azienda model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Azienda (Nome, PartitaIVA, IndirizzoSede, CittaSede, Gestione)" + "VALUES (@Nome, @PartitaIVA, @IndirizzoSede, @CittaSede, @Gestione)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", model.Nome);
                        cmd.Parameters.AddWithValue("@PartitaIVA", model.PartitaIVA);
                        cmd.Parameters.AddWithValue("@IndirizzoSede", model.IndirizzoSede);
                        cmd.Parameters.AddWithValue("@CittaSede", model.CittaSede);
                        cmd.Parameters.AddWithValue("@Gestione", model.Gestione);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Azienda aggiunta con successo!";
                return RedirectToAction("ListaAzienda");
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaAzienda(int Id)
        {
            Azienda aziendaDaEliminare = GetAziendaById(Id);

            if (aziendaDaEliminare == null)
            {
                TempData["Errore"] = "Azienda non trovato!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Azienda WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Azienda eliminata con successo!";
            }
            return RedirectToAction("ListaAzienda");
        }

        [HttpGet]
        public ActionResult ModificaAzienda(int Id)
        {
            Azienda aziendaDaModificare = GetAziendaById(Id);

            if (aziendaDaModificare == null)
            {
                TempData["Errore"] = "Azienda non trovata!";
            }

            return View(aziendaDaModificare);
        }

        [HttpPost]
        public ActionResult ModificaAzienda(Azienda aziendaModificata)
        {
            if (ModelState.IsValid)
            {
                string query = "UPDATE Azienda SET " +
                    "Nome = @Nome, " +
                    "PartitaIVA = @PartitaIVA, " +
                    "IndirizzoSede = @IndirizzoSede, " +
                    "CittaSede = @CittaSede, " +
                    "Gestione = @Gestione " +
                    "WHERE Id = @Id";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", aziendaModificata.Nome);
                        cmd.Parameters.AddWithValue("@PartitaIVA", aziendaModificata.PartitaIVA);
                        cmd.Parameters.AddWithValue("@IndirizzoSede", aziendaModificata.IndirizzoSede);
                        cmd.Parameters.AddWithValue("@CittaSede", aziendaModificata.CittaSede);
                        cmd.Parameters.AddWithValue("@Gestione", aziendaModificata.Gestione);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Azienda modificata con successo!";
            }
            return RedirectToAction("ListaAzienda");
        }

    }
}