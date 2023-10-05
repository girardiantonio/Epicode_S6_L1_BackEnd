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
    public class SpedizioneController : Controller
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        private Spedizione GetSpedizioneById(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Spedizione WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Spedizione spedizione = new Spedizione
                            {
                                Id = (int)reader["Id"],
                                ClienteId = (int)reader["ClienteId"],
                                Mittente = reader["Mittente"].ToString(),
                                CodiceSpedizione = reader["CodiceSpedizione"].ToString(),
                                NomeDestinatario = reader["NomeDestinatario"].ToString(),
                                IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString(),
                                CittaDestinazione = reader["CittaDestinazione"].ToString(),
                                Costo = (decimal)reader["Costo"],
                                PesoKg = (decimal)reader["PesoKg"],
                                DataSpedizione = reader["DataSpedizione"].ToString(),
                                DataStimataConsegna = reader["DataStimataConsegna"].ToString(),
                            };
                            return spedizione;
                        }
                        return null;
                    }
                }
            }
        }

        private Stato GetStatoById(int SpedizioneId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Stato WHERE SpedizioneId = @SpedizioneId";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@SpedizioneId", SpedizioneId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Stato stato = new Stato
                            {
                                Id = (int)reader["Id"],
                                SpedizioneId = (int)reader["SpedizioneId"],
                                Aggiornamento = reader["Aggiornamento"].ToString(),
                                Luogo = reader["Luogo"].ToString(),
                                Descrizione = reader["Descrizione"].ToString(),
                                DataOraAggiornamento = reader["DataOraAggiornamento"].ToString(),
                            };
                            return stato;
                        }
                        return null;
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult ListaSpedizione()
        {
            List<Spedizione> spedizioni = new List<Spedizione>();

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Spedizione";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Spedizione spedizione = new Spedizione
                            {
                                Id = (int)reader["Id"],
                                ClienteId = (int)reader["ClienteId"],
                                Mittente = reader["Mittente"].ToString(),
                                CodiceSpedizione = reader["CodiceSpedizione"].ToString(),
                                NomeDestinatario = reader["NomeDestinatario"].ToString(),
                                IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString(),
                                CittaDestinazione = reader["CittaDestinazione"].ToString(),
                                Costo = (decimal)reader["Costo"],
                                PesoKg = (decimal)reader["PesoKg"],
                                DataSpedizione = reader["DataSpedizione"].ToString(),
                                DataStimataConsegna = reader["DataStimataConsegna"].ToString(),
                            };

                            spedizioni.Add(spedizione);
                        }
                    }
                }
            }
            return View(spedizioni);
        }

        [HttpGet]
        public ActionResult AggiungiSpedizione(int ClienteId, string Mittente)
        {
            ViewBag.ClienteId = ClienteId;
            ViewBag.Mittente = Mittente;
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiSpedizione(Spedizione model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Spedizione (ClienteId, Mittente, CodiceSpedizione, NomeDestinatario, IndirizzoDestinazione, CittaDestinazione, Costo, PesoKg, DataSpedizione, DataStimataConsegna)" + "VALUES (@ClienteId, @Mittente, @CodiceSpedizione, @NomeDestinatario, @IndirizzoDestinazione, @CittaDestinazione, @Costo, @PesoKg, @DataSpedizione, @DataStimataConsegna)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@ClienteId", model.ClienteId);
                        cmd.Parameters.AddWithValue("@Mittente", model.Mittente);
                        cmd.Parameters.AddWithValue("@CodiceSpedizione", model.CodiceSpedizione);
                        cmd.Parameters.AddWithValue("@NomeDestinatario", model.NomeDestinatario);
                        cmd.Parameters.AddWithValue("@IndirizzoDestinazione", model.IndirizzoDestinazione);
                        cmd.Parameters.AddWithValue("@CittaDestinazione", model.CittaDestinazione);
                        cmd.Parameters.AddWithValue("@Costo", model.Costo);
                        cmd.Parameters.AddWithValue("@PesoKg", model.PesoKg);
                        cmd.Parameters.AddWithValue("@DataSpedizione", model.DataSpedizione);
                        cmd.Parameters.AddWithValue("@DataStimataConsegna", model.DataStimataConsegna);

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("ListaSpedizione");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DettaglioSpedizione(int Id)
        {
            Spedizione dettaglioSpedizione = GetSpedizioneById(Id);

            if (dettaglioSpedizione == null)
            {
                TempData["Errore"] = "Spedizione non trovata!";
                return RedirectToAction("CercaSpedizione", "Spedizione");
            }

            Stato statoSpedizione = GetStatoById(dettaglioSpedizione.Id);

            var dettaglioModel = new DettaglioSpedizione
            {
                Spedizione = dettaglioSpedizione,
                Stato = statoSpedizione
            };

            return View(dettaglioModel);
        }


        [HttpGet]
        public ActionResult CercaSpedizione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CercaSpedizione(string codiceSpedizione, string Mittente)
        {
            if (ModelState.IsValid)
            {
                string query = "SELECT * FROM Spedizione WHERE CodiceSpedizione = @CodiceSpedizione AND Mittente = @Mittente";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@CodiceSpedizione", codiceSpedizione);
                    command.Parameters.AddWithValue("@Mittente", Mittente);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var spedizione = new Spedizione
                            {
                                Id = (int)reader["Id"],
                                ClienteId = (int)reader["ClienteId"],
                                Mittente = reader["Mittente"].ToString(),
                                CodiceSpedizione = reader["CodiceSpedizione"].ToString(),
                                NomeDestinatario = reader["NomeDestinatario"].ToString(),
                                IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString(),
                                CittaDestinazione = reader["CittaDestinazione"].ToString(),
                                Costo = (decimal)reader["Costo"],
                                PesoKg = (decimal)reader["PesoKg"],
                                DataSpedizione = reader["DataSpedizione"].ToString(),
                                DataStimataConsegna = reader["DataStimataConsegna"].ToString()
                            };

                            return RedirectToAction("StatoSpedizione", "Stato", new { id = spedizione.Id });
                        }
                        else
                        {
                            return View();
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult AggiungiStato()
        {
            return View();
        }

    }
}