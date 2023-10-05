using Epicode_S6_L1_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Epicode_S6_L1_BackEnd.Controllers
{
    [Authorize(Roles = "admin")]
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

        private List<Stato> GetListStatoById(int SpedizioneId)
        {
            List<Stato> ListaStato = new List<Stato>();

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Stato WHERE SpedizioneId = @SpedizioneId ORDER BY DataOraAggiornamento DESC";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@SpedizioneId", SpedizioneId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Stato stato = new Stato
                            {
                                Id = (int)reader["Id"],
                                SpedizioneId = (int)reader["SpedizioneId"],
                                Aggiornamento = reader["Aggiornamento"].ToString(),
                                Luogo = reader["Luogo"].ToString(),
                                Descrizione = reader["Descrizione"].ToString(),
                                DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"],
                            };
                            ListaStato.Add(stato);
                        }
                    }
                }
                return ListaStato;
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
                                DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"],
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
        [AllowAnonymous]
        public ActionResult DettaglioSpedizioneCliente(int Id)
        {
            Spedizione dettaglioSpedizione = GetSpedizioneById(Id);

            if (dettaglioSpedizione == null)
            {
                TempData["Errore"] = "Spedizione non trovata!";
                return RedirectToAction("CercaSpedizione", "Spedizione");
            }

            List<Stato> ListaStato = GetListStatoById(dettaglioSpedizione.Id);

            List<DettaglioSpedizione> dettaglioModel = new List<DettaglioSpedizione>();

            DettaglioSpedizione dettaglio = new DettaglioSpedizione
            {
                Spedizione = dettaglioSpedizione,
                Stato = ListaStato
            };

            dettaglioModel.Add(dettaglio);

            return View(dettaglioModel);
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

            List<Stato> ListaStato = GetListStatoById(dettaglioSpedizione.Id);

            List<DettaglioSpedizione> dettaglioModel = new List<DettaglioSpedizione>();

            DettaglioSpedizione dettaglio = new DettaglioSpedizione
            {
                Spedizione = dettaglioSpedizione,
                Stato = ListaStato
            };

            dettaglioModel.Add(dettaglio);

            return View(dettaglioModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CercaSpedizione()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CercaSpedizione(string Mittente, string CodiceSpedizione)
        {
            if (ModelState.IsValid)
            {
                string query = "SELECT * FROM Spedizione WHERE Mittente = @Mittente AND CodiceSpedizione = @CodiceSpedizione";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@Mittente", Mittente);
                    command.Parameters.AddWithValue("@CodiceSpedizione", CodiceSpedizione);

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

                            return RedirectToAction("DettaglioSpedizione", new { Id = spedizione.Id });
                        }
                        else
                        {
                            ViewBag.AuthError = "Spedizione non trovata, verifica i tuoi dati";
                            return View();
                        }
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult AggiungiStato(int SpedizioneId)
        {
            ViewBag.SpedizioneId = SpedizioneId;
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiStato(Stato model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.DataOraAggiornamento = DateTime.Now;

                    string query = "INSERT INTO Stato (SpedizioneId, Aggiornamento, Luogo, Descrizione, DataOraAggiornamento)" +
                                   "VALUES (@SpedizioneId, @Aggiornamento, @Luogo, @Descrizione, @DataOraAggiornamento)";

                    using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@SpedizioneId", model.SpedizioneId);
                            cmd.Parameters.AddWithValue("@Aggiornamento", model.Aggiornamento);
                            cmd.Parameters.AddWithValue("@Luogo", model.Luogo);
                            cmd.Parameters.AddWithValue("@Descrizione", model.Descrizione);
                            cmd.Parameters.AddWithValue("@DataOraAggiornamento", model.DataOraAggiornamento);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("DettaglioSpedizione", new { Id = model.SpedizioneId });
                }
                catch (Exception ex)
                {
                    ViewBag.AuthError = "Parametri non validi";
                    return View();
                }
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult ModificaSpedizione(int Id)
        {
            Spedizione spedizione = GetSpedizioneById(Id);

            if (spedizione == null)
            {
                TempData["Errore"] = "Spedizione non trovata!";
                return RedirectToAction("CercaSpedizione", "Spedizione");
            }

            return View(spedizione);
        }


        [HttpPost]
        public ActionResult ModificaSpedizione(Spedizione model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();

                        string query = "UPDATE Spedizione SET " +
                                       "CodiceSpedizione = @CodiceSpedizione, " +
                                       "NomeDestinatario = @NomeDestinatario, " +
                                       "IndirizzoDestinazione = @IndirizzoDestinazione, " +
                                       "CittaDestinazione = @CittaDestinazione, " +
                                       "Costo = @Costo, " +
                                       "PesoKg = @PesoKg, " +
                                       "DataSpedizione = @DataSpedizione, " +
                                       "DataStimataConsegna = @DataStimataConsegna " +
                                       "WHERE Id = @Id";

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@Id", model.Id);
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
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la modifica della spedizione.");
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult EliminaSpedizione(int Id)
        {
            Spedizione spedizioneDaEliminare = GetSpedizioneById(Id);

            if (spedizioneDaEliminare == null)
            {
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string deleteStatoQuery = "DELETE FROM Stato WHERE SpedizioneId = @SpedizioneId";

                    using (SqlCommand deleteStatoCmd = new SqlCommand(deleteStatoQuery, sqlConnection))
                    {
                        deleteStatoCmd.Parameters.AddWithValue("@SpedizioneId", spedizioneDaEliminare.Id);
                        deleteStatoCmd.ExecuteNonQuery();
                    }

                    string deleteSpedizioneQuery = "DELETE FROM Spedizione WHERE Id = @Id";

                    using (SqlCommand deleteSpedizioneCmd = new SqlCommand(deleteSpedizioneQuery, sqlConnection))
                    {
                        deleteSpedizioneCmd.Parameters.AddWithValue("@Id", spedizioneDaEliminare.Id);
                        deleteSpedizioneCmd.ExecuteNonQuery();
                    }

                }
            }
            return RedirectToAction("ListaSpedizione");
        }

        [HttpGet]
        public ActionResult ModificaStato(int SpedizioneId)
        {
            ViewBag.SpedizioneId = SpedizioneId;
            return View();
        }

        [HttpPost]
        public ActionResult ModificaStato(Stato model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.DataOraAggiornamento = DateTime.Now;

                    string query = "UPDATE Stato SET Aggiornamento = @Aggiornamento, Luogo = @Luogo, Descrizione = @Descrizione, DataOraAggiornamento = @DataOraAggiornamento WHERE Id = @Id";

                    using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@Aggiornamento", model.Aggiornamento);
                            cmd.Parameters.AddWithValue("@Luogo", model.Luogo);
                            cmd.Parameters.AddWithValue("@Descrizione", model.Descrizione);
                            cmd.Parameters.AddWithValue("@DataOraAggiornamento", model.DataOraAggiornamento);
                            cmd.Parameters.AddWithValue("@Id", model.Id);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("ListaSpedizione");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiornamento dello stato.");
                }
            }
            return View(model);
        }





    }
}