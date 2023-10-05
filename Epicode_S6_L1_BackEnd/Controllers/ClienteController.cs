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
    public class ClienteController : Controller
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        private Cliente GetClienteById(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Clienti WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                LuogoDiNascita = reader["LuogoDiNascita"].ToString(),
                                Residenza = reader["Residenza"].ToString(),
                                DataDiNascita = reader["DataDiNascita"].ToString(),
                                IsAzienda = (bool)reader["IsAzienda"],
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                PartitaIVA = reader["PartitaIVA"].ToString(),
                                IndirizzoSede = reader["IndirizzoSede"].ToString(),
                                CittaSede = reader["CittaSede"].ToString(),
                            };
                            return cliente;
                        }
                        return null;
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult ListaCliente()
        {
            List<Cliente> clienti = Cliente.ListaClienti();
            return View(clienti);
        }

        [HttpGet]
        public ActionResult AggiungiCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiCliente(Cliente model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsAzienda)
                {
                    string query = "INSERT INTO Clienti (Nome, PartitaIVA, IndirizzoSede, CittaSede, IsAzienda)" + "VALUES (@Nome, @PartitaIVA, @IndirizzoSede, @CittaSede, @IsAzienda)";

                    using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@Nome", model.Nome);
                            cmd.Parameters.AddWithValue("@PartitaIVA", model.PartitaIVA);
                            cmd.Parameters.AddWithValue("@IndirizzoSede", model.IndirizzoSede);
                            cmd.Parameters.AddWithValue("@CittaSede", model.CittaSede);
                            cmd.Parameters.AddWithValue("@IsAzienda", true);

                            cmd.ExecuteNonQuery();
                        }
                    }
                } else
                {
                    string query = "INSERT INTO Clienti (Nome, Cognome, CodiceFiscale, LuogoDiNascita, Residenza, DataDiNascita, IsAzienda)" + "VALUES (@Nome, @Cognome, @CodiceFiscale, @LuogoDiNascita, @Residenza, @DataDiNascita, @IsAzienda)";

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
                            cmd.Parameters.AddWithValue("@IsAzienda", false);

                            cmd.ExecuteNonQuery();
                        }
                    }


                }
                return RedirectToAction("ListaCliente");
            }
            return View("AggiungiCliente", model);
        }

        [HttpGet]
        public ActionResult ModificaCliente(int Id)
        {
            Cliente clienteDaModificare = GetClienteById(Id);

            if (clienteDaModificare == null)
            {
                //return aggiungere alert
            }

            return View(clienteDaModificare);
        }

        [HttpPost]
        public ActionResult ModificaCliente(Cliente clienteModificato)
        {
            if (ModelState.IsValid)
            {
                if (clienteModificato.IsAzienda == true)
                {
                    string query = "UPDATE Clienti SET " +
                                    "Nome = @Nome, " +
                                    "PartitaIVA = @PartitaIVA, " +
                                    "IndirizzoSede = @IndirizzoSede, " +
                                    "CittaSede = @CittaSede " +
                                    "WHERE Id = @Id";

                    using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@Id", clienteModificato.Id);
                            cmd.Parameters.AddWithValue("@Nome", clienteModificato.Nome);
                            cmd.Parameters.AddWithValue("@PartitaIVA", clienteModificato.PartitaIVA);
                            cmd.Parameters.AddWithValue("@IndirizzoSede", clienteModificato.IndirizzoSede);
                            cmd.Parameters.AddWithValue("@CittaSede", clienteModificato.CittaSede);
                            cmd.Parameters.AddWithValue("@IsAzienda", true);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    string query = "UPDATE Clienti SET " +
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
                            cmd.Parameters.AddWithValue("@Id", clienteModificato.Id);
                            cmd.Parameters.AddWithValue("@Nome", clienteModificato.Nome);
                            cmd.Parameters.AddWithValue("@Cognome", clienteModificato.Cognome);
                            cmd.Parameters.AddWithValue("@CodiceFiscale", clienteModificato.CodiceFiscale);
                            cmd.Parameters.AddWithValue("@LuogoDiNascita", clienteModificato.LuogoDiNascita);
                            cmd.Parameters.AddWithValue("@Residenza", clienteModificato.Residenza);
                            cmd.Parameters.AddWithValue("@DataDiNascita", clienteModificato.DataDiNascita);
                            cmd.Parameters.AddWithValue("@IsAzienda", false);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return RedirectToAction("ListaCliente");
            }
            return View("AggiungiCliente", clienteModificato);
        }

        [HttpGet]
        public ActionResult EliminaCliente(int Id)
        {
            Cliente clienteDaEliminare = GetClienteById(Id);

            if (clienteDaEliminare == null)
            {
                //return aggiungere alert
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Clienti WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                //return aggiungere alert
            }
            return RedirectToAction("ListaCliente");
        }
    }
}