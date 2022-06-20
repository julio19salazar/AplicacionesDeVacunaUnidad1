using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MiAgenda.Models
{
   public class AgendaModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Actividad { get; set; }
        public string FechaDeRealizar { get; set; }
        public string Importancia { get; set; }
        [Ignore]
        public ulong Eliminado { get; set; }
        public DateTime Timestamp { get; set; }
        
      
    }
}
