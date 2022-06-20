using System;
using System.Collections.Generic;

#nullable disable

namespace ApiVacunacion.Models
{
    public partial class Agenda
    {
        public int Id { get; set; }
        public string Actividad { get; set; }
        public string FechaDeRealizar { get; set; }
        public string Importancia { get; set; }
        public ulong Eliminado { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
