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

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Spedizione";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Spedizione spedizione = new Spedizione
                            {
                                Id = (int)reader["Id"],
                                ClienteId = (int)reader["ClienteId"],
                                NomeDestinatario = reader["NomeDestinatario"].ToString(),
                                IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString(),
                                CittaDestinazione = reader["CittaDestinazione"].ToString(),
                                Costo = (decimal)reader["Costo"],
                                PesoKg = (decimal)reader["PesoKg"],
                                DataSpedizione = (DateTime)reader["DataSpedizione"],
                                DataStimataConsegna = (DateTime)reader["DataStimataConsegna"]
                            };
                        };

                            spedizioni.Add(spedizione);
                        }
                    }
                }

            }
            return View(spedizioni);
        }
    }
}