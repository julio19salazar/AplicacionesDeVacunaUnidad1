using System;
using System.Collections.Generic;

#nullable disable

namespace ApiVacunacion.Models
{
    public partial class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
        public string Vacuna { get; set; }
        public string Lote { get; set; }
        public ulong Eliminado { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
