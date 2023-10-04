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
        public string NomeDestinatario { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string CittaDestinazione { get; set; }
        public decimal Costo { get; set; }
        public decimal PesoKg { get; set; }
        public DateTime DataSpedizione { get; set; }
        public DateTime DataStimataConsegna { get; set; }


        public Cliente Cliente { get; set; }
    }

}


