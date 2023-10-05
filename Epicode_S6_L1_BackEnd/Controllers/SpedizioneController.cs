using Epicode_S6_L1_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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
    }
}