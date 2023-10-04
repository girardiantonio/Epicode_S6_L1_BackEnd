using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicode_S6_L1_BackEnd.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string LuogoDiNascita { get; set; }
        public string Residenza { get; set; }
        public DateTime DataDiNascita { get; set; }
        public bool IsAzienda { get; set; } 
        public string CodiceFiscale { get; set; } // Codice Fiscale per i privati
        public string PartitaIVA { get; set; } // Partita IVA per le aziende
        public string IndirizzoSede { get; set; } // Indirizzo sede per le aziende
        public string CittaSede { get; set; } // Città sede per le aziende
    }
}