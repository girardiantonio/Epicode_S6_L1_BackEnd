using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicode_S6_L1_BackEnd.Models
{
    public class Utente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string CodiceFiscale { get; set; }
        public string LuogoDiNascita { get; set; }
        public string Residenza { get; set; }
        public DateTime DataDiNascita { get; set; }
    }

}