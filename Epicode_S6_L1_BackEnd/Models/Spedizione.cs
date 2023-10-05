using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Epicode_S6_L1_BackEnd.Models
{
    public class Spedizione
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Mittente { get; set; }    
        public string CodiceSpedizione { get; set; }
        public string NomeDestinatario { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string CittaDestinazione { get; set; }
        public decimal Costo { get; set; }
        public decimal PesoKg { get; set; }
        public string DataSpedizione { get; set; }
        public string DataStimataConsegna { get; set; }
    }
    public class Stato
    {
        public int Id { get; set; }
        public int SpedizioneId { get; set; }
        public string Aggiornamento { get; set; }
        public string Luogo { get; set; }
        public string Descrizione { get; set; }
        public string DataOraAggiornamento { get; set; }

    }

    public class DettaglioSpedizione
    {
        public Spedizione Spedizione { get; set; }
        public Stato Stato { get; set; }
    }
}


