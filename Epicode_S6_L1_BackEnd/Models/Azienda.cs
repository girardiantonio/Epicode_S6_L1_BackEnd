using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicode_S6_L1_BackEnd.Models
{
    public class Azienda
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string PartitaIVA { get; set; }
        public string IndirizzoSede { get; set; }
        public string CittaSede { get; set; }
        public string Gestione { get; set; }
    }
}